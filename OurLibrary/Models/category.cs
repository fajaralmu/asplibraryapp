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
    public partial class category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public category()
        {
            this.books = new HashSet<book>();
        }

        [FieldAttribute(AutoGenerated = true, Required = true, FieldType = AttributeConstant.TYPE_ID, FixSize = 4)]

        public string id { get; set; }
        [FieldAttribute(Required = true, FieldName = "Category Name", FieldType = AttributeConstant.TYPE_TEXTBOX)]
        public string category_name { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<book> books { get; set; }
    }
}
