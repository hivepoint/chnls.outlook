#region

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#endregion

namespace chnls.Controls
{
    /// <summary>
    ///     Represents a Windows text box control with placeholder.
    /// </summary>
    public class PlaceholderTextBox : TextBox
    {
        #region Properties

        private bool _isItalics = true;
        private bool _isPlaceholderActive;
        private string _placeholderText = DEFAULT_PLACEHOLDER;


        /// <summary>
        ///     Gets a value indicating whether the Placeholder is active.
        /// </summary>
        [Browsable(false)]
        public bool IsPlaceholderActive
        {
            get { return _isPlaceholderActive; }
            private set
            {
                if (value != _isPlaceholderActive)
                {
                    _isPlaceholderActive = value;

                    if (PlaceholderInsideChanged != null)
                        PlaceholderInsideChanged(this, new PlaceholderInsideChangedEventArgs(value));
                }
            }
        }


        /// <summary>
        ///     Gets or sets a value indicating whether the font of the placeholder is italics.
        /// </summary>
        [Description("Specifies whether the placeholder text is italics."), Category("Appearance"), DefaultValue(true)]
        public bool IsItalics
        {
            get { return _isItalics; }
            set
            {
                _isItalics = value;

                // If placeholder is active, assign new FontStyle
                if (IsPlaceholderActive)
                    AssignPlaceholderStyle();
            }
        }


        /// <summary>
        ///     Gets or sets the current placeholder in the Demo.PlaceholderTextBox.
        /// </summary>
        [Description("The placeholder associated with the control."), Category("Appearance"),
         DefaultValue(DEFAULT_PLACEHOLDER)]
        public string PlaceholderText
        {
            get { return _placeholderText; }
            set
            {
                _placeholderText = value;

                // Only use the new value if the placeholder is active.
                if (IsPlaceholderActive)
                    Text = value;
            }
        }


        /// <summary>
        ///     Gets or sets the current text in the Demo.TextBox.
        /// </summary>
        [Browsable(false)]
        public override string Text
        {
            get
            {
                // Check 'IsPlaceholderActive' to avoid this if-clause when the text is the same as the placeholder but actually it's not the placeholder.
                // Check 'base.Text == this.Placeholder' because in some cases IsPlaceholderActive changes too late although it isn't the placeholder anymore.
                // If you want to get the Text Property and it still contains the playerholder, an empty string will get returned.
                if (IsPlaceholderActive && base.Text == PlaceholderText)
                    return String.Empty;

                return base.Text;
            }
            set { base.Text = value; }
        }


        /// <summary>
        ///     Occurs when the value of the IsPlaceholderInside property has changed.
        /// </summary>
        [Description("Occurs when the value of the IsPlaceholderInside property has changed.")]
        public event EventHandler<PlaceholderInsideChangedEventArgs> PlaceholderInsideChanged;

        #endregion

        #region Global Variables

        /// <summary>
        ///     Specifies the default placeholder text.
        /// </summary>
        private const string DEFAULT_PLACEHOLDER = "<Input>";

        /// <summary>
        ///     Specifies the regular selected Font (usually specified by Designer).
        /// </summary>
        private readonly Font regularFont;

        /// <summary>
        ///     Specifies the regular selected FontColor (usually specified by Designer).
        /// </summary>
        private readonly Color regularFontColor;

        /// <summary>
        ///     Flag to avoid the TextChanged Event. Don't access directly, use Method 'ActionWithoutTextChanged(Action act)'
        ///     instead.
        /// </summary>
        private bool avoidTextChanged;

        #endregion

        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the Demo.PlaceholderTextBox class.
        /// </summary>
        public PlaceholderTextBox()
        {
            // Set Default
            IsPlaceholderActive = true;

            // Through this line the default placeholder gets displayed in designer
            Text = PlaceholderText;

            // Save Font
            regularFont = (Font) Font.Clone();
            regularFontColor = ForeColor;

            SubscribeEvents();
            AssignPlaceholderStyle();
        }

        #endregion

        #region Functions

        /// <summary>
        ///     Insert Placeholder and assign Placeholder Style.
        /// </summary>
        public void Reset()
        {
            AssignPlaceholderStyle();

            ActionWithoutTextChanged(() => Text = PlaceholderText);
            Select(0, 0);
        }

        /// <summary>
        ///     Run an action with avoiding the TextChanged event.
        /// </summary>
        /// <param name="act">Specifies the action to run.</param>
        private void ActionWithoutTextChanged(Action act)
        {
            avoidTextChanged = true;

            act.Invoke();

            avoidTextChanged = false;
        }

        /// <summary>
        ///     Set style to "Placeholder-Style".
        /// </summary>
        private void AssignPlaceholderStyle()
        {
            // Set classic placeholder style
            Font = IsItalics ? new Font(Font, FontStyle.Italic) : new Font(Font, FontStyle.Regular);
            ForeColor = Color.LightGray;

            // Update IsPlayerholderInside property
            IsPlaceholderActive = true;
        }

        /// <summary>
        ///     Remove "Placeholder-Style".
        /// </summary>
        private void RemovePlaceholderStyle()
        {
            // Revert to designer specified font
            Font = regularFont;
            ForeColor = regularFontColor;

            // Update IsPlaceholderInside property
            IsPlaceholderActive = false;
        }

        /// <summary>
        ///     Subscribe necessary Events.
        /// </summary>
        private void SubscribeEvents()
        {
            TextChanged += PlaceholderTextBox_TextChanged;
        }

        #endregion

        #region Events

        private void PlaceholderTextBox_TextChanged(object sender, EventArgs e)
        {
            // Check flag
            if (avoidTextChanged)
                return;

            // Run code with avoiding recursive call
            ActionWithoutTextChanged(delegate
            {
                // If the Text is empty, insert placeholder and set cursor to to first position
                if (String.IsNullOrEmpty(Text))
                {
                    Reset();
                    return;
                }

                // If the placeholder is active, revert state to a usual TextBox
                if (IsPlaceholderActive)
                {
                    RemovePlaceholderStyle();

                    // Throw away the placeholder but leave the new typed char
                    Text = Text.Replace(PlaceholderText, String.Empty);

                    // Set Selection to last position
                    Select(TextLength, 0);
                }
            });
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            // When you click on the placerholderTextBox and the placerholder is active, jump to first position
            if (IsPlaceholderActive)
                Reset();

            base.OnMouseDown(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            // Prevents that the user can go through the placeholder with arrow keys
            if ((e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down) &&
                IsPlaceholderActive)
                e.Handled = true;

            base.OnKeyDown(e);
        }

        #endregion
    }

    /// <summary>
    ///     Provides data for the Demo.PlaceholderInsideChanged event.
    /// </summary>
    public class PlaceholderInsideChangedEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the Demo.PlaceholderInsideChangedEventArgs class.
        /// </summary>
        /// <param name="newValue">The new value of the IsPlaceholderInside Property.</param>
        public PlaceholderInsideChangedEventArgs(bool newValue)
        {
            NewValue = newValue;
        }

        /// <summary>
        ///     The new value of the IsPlaceholderInside Property.
        /// </summary>
        public bool NewValue { get; private set; }
    }
}