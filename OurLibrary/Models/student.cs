//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OurLibrary.Models
{
    using Annotation;
    using System;
    using System.Collections.Generic;
    [Serializable]
    public partial class student
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public student()
        {
            this.issues = new HashSet<issue>();
        }

        [FieldAttribute(AutoGenerated = true, Required = true, FieldType = AttributeConstant.TYPE_ID, FixSize = 10)]

        public string id { get; set; }
        [FieldAttribute(Required = true, FieldType = AttributeConstant.TYPE_TEXTBOX)]

        public string name { get; set; }
        [FieldAttribute(Required = true, FieldType = AttributeConstant.TYPE_DATE, FieldName = "Birth Date")]

        public string bod { get; set; }
        [FieldAttribute(Required = true, FieldType = AttributeConstant.TYPE_DROPDOWN, ClassReference = "class", ClassAttributeConverter = "class_name", FieldName = "Kelas")]

        public string class_id { get; set; }
        [FieldAttribute(Required = false, FieldType = AttributeConstant.TYPE_TEXTBOX)]

        public string email { get; set; }
        [FieldAttribute(Required = false, FieldType = AttributeConstant.TYPE_TEXTAREA)]
        public string address { get; set; }

        public virtual @class @class { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<issue> issues { get; set; }
    }
}
