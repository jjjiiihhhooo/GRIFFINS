using UnityEngine;

namespace MoreMountains.Tools
{
    public class MMDropdownAttribute : PropertyAttribute
    {
        public readonly object[] DropdownValues;

        public MMDropdownAttribute(params object[] dropdownValues)
        {
            DropdownValues = dropdownValues;
        }
    }
}