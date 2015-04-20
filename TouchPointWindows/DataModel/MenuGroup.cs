using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TouchPointWindows.DataModel
{
    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class MenuGroup
    {
        public MenuGroup()
        {
            
        }
        public MenuGroup(String name, string parentName)
        {
            this.Name = name;
            ParentName = parentName;
            this.Items = new List<Menu>();
            this.ChildGroups = new List<MenuGroup>();
        }
        public MenuGroup(String name, int parentId)
        {
            this.Name = name;
            ParentId = parentId;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string ParentName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public List<Menu> Items { get; set; }
        public List<MenuGroup> ChildGroups { get; set; }
        public override string ToString()
        {
            return this.Name;
        }
    }
}