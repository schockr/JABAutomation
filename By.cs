using System;
using System.Collections.ObjectModel;

namespace JABAutomation
{
    public class By
    {
        private static readonly string XPathSelectorMechanism = SelectorMechanisms.XPath;
        private static readonly string PushButtonNameSelectorMechanism = SelectorMechanisms.PushButtonName;
        private static readonly string LabelTextSelectorMechanism = SelectorMechanisms.LabelText;
        private static readonly string PartialLabelTextSelectorMechanism = SelectorMechanisms.PartialLabelText;
        private static readonly string ComboboxNameSelectorMechanism = SelectorMechanisms.ComboboxName;
        private static readonly string NameSelectorMechanism = SelectorMechanisms.Name;
        private static readonly string NameAndRoleSelectorMechanism = SelectorMechanisms.NameAndRole;
        private static readonly string DescriptionSelectorMechanism = SelectorMechanisms.Description;
        private static readonly string DescriptionAndRoleSelectorMechanism = SelectorMechanisms.DescriptionAndRole;
        private static readonly string TextboxNameSelectorMechanism = SelectorMechanisms.TextboxName;
        private static readonly string LabelNameSelectorMechanism = SelectorMechanisms.LabelName;
        private static readonly string RadioButtonNameSelectorMechanism = SelectorMechanisms.RadioButtonName;
        private static readonly string ToggleButtonNameSelectorMechanism = SelectorMechanisms.ToggleButtonName;

        private string desc = "JABAutomation.By";
        private string mechanism = string.Empty;
        private string criteria = string.Empty;
        private ElementType role = null;

        public string Mechanism
        {
            get { return this.mechanism; }
        }
        public string Criteria
        {
            get { return this.criteria; }
        }

        public ElementType Role
        {
            get { return this.role; }
        }

        protected string Desc
        {
            get { return this.desc; }
            set { this.desc = value; }
        }

        protected By()
        {
        }

        protected By(string mechanism, string criteria)
        {
            this.mechanism = mechanism;
            this.criteria = criteria;
        }

        protected By(string mechanism, string criteria, ElementType role)
        {
            this.mechanism = mechanism;
            this.criteria = criteria;
            this.role = role;
        }

        public static By XPath(string xpathToFind)
        {
            if (xpathToFind == null)
            {
                throw new ArgumentNullException(nameof(xpathToFind), "Cannot find elements when the XPath expression is null.");
            }

            By by = new By(XPathSelectorMechanism, xpathToFind);
            by.desc = "By.XPath: " + xpathToFind;
            return by;
        }

        public static By PushButtonName(string btnName)
        {
            if (btnName == null)
            {
                throw new ArgumentNullException(nameof(btnName), "Cannot find elements when the push button name is null.");
            }

            By by = new By(PushButtonNameSelectorMechanism, btnName);
            by.desc = "By.PushButtonName: " + btnName;
            return by;
        }

        public static By LabelText(string labelText, bool partial = false)
        {
            if (labelText == null)
            {
                throw new ArgumentNullException(nameof(labelText), "Cannot find elements when the label text is null.");
            }

            if (partial)
            {
                By partialBy = new By(PartialLabelTextSelectorMechanism, labelText);
                partialBy.desc = "By.PartialLabelText: " + labelText;
                return partialBy;
            }

            By by = new By(LabelTextSelectorMechanism, labelText);
            by.desc = "By.LabelText: " + labelText;
            return by;
        }

        public static By ComboboxName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name), "Cannot find elements when the combobox name is null.");
            }

            By by = new By(ComboboxNameSelectorMechanism, name);
            by.desc = "By.ComboboxName: " + name;
            return by;
        }

        public static By Name(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name), "Cannot find elements when the element name is null.");
            }

            By by = new By(NameSelectorMechanism, name);
            by.desc = "By.Name: " + name;
            return by;
        }
        public static By NameAndRole(string name, ElementType role)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name), "Cannot find elements when the element name is null.");
            }

            By by = new By(NameAndRoleSelectorMechanism, name, role);
            by.desc = "By.NameAndRole: " + name + ", "+role;
            return by;
        }
        public static By Description(string desc)
        {
            if (desc == null)
            {
                throw new ArgumentNullException(nameof(desc), "Cannot find elements when the element description is null.");
            }

            By by = new By(DescriptionSelectorMechanism, desc);
            by.desc = "By.Description: " + desc;
            return by;
        }
        public static By DescriptionAndRole(string desc, ElementType role)
        {
            if (desc == null)
            {
                throw new ArgumentNullException(nameof(desc), "Cannot find elements when the element description is null.");
            }

            By by = new By(DescriptionAndRoleSelectorMechanism, desc, role);
            by.desc = "By.DescriptionAndRole: " + desc + ", " + role;
            return by;
        }


        public static By TextboxName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name), "Cannot find elements when the textbox name is null.");
            }

            By by = new By(TextboxNameSelectorMechanism, name);
            by.desc = "By.TextboxName: " + name;
            return by;
        }

        public static By LabelName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name), "Cannot find elements when the label name is null.");
            }

            By by = new By(LabelNameSelectorMechanism, name);
            by.desc = "By.LabelName: " + name;
            return by;
        }
        public static By RadioButtonName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name), "Cannot find elements when the radio button name is null.");
            }

            By by = new By(RadioButtonNameSelectorMechanism, name);
            by.desc = "By.RadioButtonName: " + name;
            return by;
        }
        public static By ToggleButtonName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name), "Cannot find elements when the toggle button name is null.");
            }

            By by = new By(ToggleButtonNameSelectorMechanism, name);
            by.desc = "By.ToggleButtonName: " + name;
            return by;
        }


        /// <summary>
        /// Determines if two <see cref="By"/> instances are equal.
        /// </summary>
        /// <param name="one">One instance to compare.</param>
        /// <param name="two">The other instance to compare.</param>
        /// <returns><see langword="true"/> if the two instances are equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(By one, By two)
        {
            // If both are null, or both are same instance, return true.
            if (object.ReferenceEquals(one, two))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)one == null) || ((object)two == null))
            {
                return false;
            }

            return one.Equals(two);
        }

        /// <summary>
        /// Determines if two <see cref="By"/> instances are unequal.
        /// </summary>s
        /// <param name="one">One instance to compare.</param>
        /// <param name="two">The other instance to compare.</param>
        /// <returns><see langword="true"/> if the two instances are not equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(By one, By two)
        {
            return !(one == two);
        }


        /// <summary>
        /// Gets a string representation of the finder.
        /// </summary>
        /// <returns>The string displaying the finder content.</returns>
        public override string ToString()
        {
            return this.desc;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object">Object</see> is equal
        /// to the current <see cref="object">Object</see>.
        /// </summary>
        /// <param name="obj">The <see cref="object">Object</see> to compare with the
        /// current <see cref="object">Object</see>.</param>
        /// <returns><see langword="true"/> if the specified <see cref="object">Object</see>
        /// is equal to the current <see cref="object">Object</see>; otherwise,
        /// <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            var other = obj as By;

            // TODO(dawagner): This isn't ideal
            return other != null && this.desc.Equals(other.desc);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see cref="object">Object</see>.</returns>
        public override int GetHashCode()
        {
            return this.desc.GetHashCode();
        }

    }
}
