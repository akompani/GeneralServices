using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralServices.Tree
{
    public class Element
    {
        public int Id { get; set; }
        public int? ParentId { get; set; } // Nullable to handle root elements
        public string Name { get; set; }
    }

    public class TreeNodeData
    {
        public TreeNodeData(string name, string imageUrl=null)
        {
            Name = name;
            ImageUrl = imageUrl;
        }

        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }

    public class TreeNodeOptions
    {
        public TreeNodeOptions()
        {
            
        }

        public TreeNodeOptions(string color)
        {
            NodeBGColor=color;
            NodeBGColorHover = color;
        }

        public string NodeBGColor { get; set; } = "#84a59d";
        public string NodeBGColorHover { get; set; } = "#c9cba3";
        
    }

    public class TreeNode
    {
        public int Id { get; set; }
        public TreeNodeData Data { get; set; }
        public TreeNodeOptions Options { get; set; }
        public TreeNode[] Children { get; set; }
    }

    public class TreeBuilder
    {
        public string[] _treeColors = new string[] { "#cdb4db", "#ffafcc", "#f8ad9d", "#c9cba3" };
        
        public TreeNode[] BuildTree(List<Element> elements)
        {
            var lookup = elements.ToLookup(e => e.ParentId); // Group by ParentId


            return BuildNodes(lookup, null);
        }

        private TreeNode[] BuildNodes(ILookup<int?, Element> lookup, int? parentId)
        {
            return lookup[parentId]
                .Select(e => new TreeNode
                {
                    Id = e.Id,
                    Data = new TreeNodeData(e.Name),
                    Options = new TreeNodeOptions(),
                    Children = BuildNodes(lookup, e.Id)
                })
                .ToArray();
        }
    }
}
