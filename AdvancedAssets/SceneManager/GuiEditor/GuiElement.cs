using System.Collections.Generic;
using System.Linq;
using ArtCore_Editor.Enums;
using Newtonsoft.Json;

namespace ArtCore_Editor.AdvancedAssets.SceneManager.GuiEditor
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GuiElement
    {
        [JsonProperty]
        private readonly List<GuiElement> _children = new List<GuiElement>();
        // static id to make all instances unique
        [JsonProperty]
        private static int _uid = 0;
        // gui element using path to know own place in tree
        //private readonly string _path;
        [JsonIgnore]
        private GuiElement _parent;

        [JsonProperty]
        private string Name { get; }

        [JsonProperty]
        public Dictionary<string, string> EventsList = new Dictionary<string, string>();

        [JsonProperty]
        // default variables in every gui element
        private readonly List<Variable> _elementVariables = new List<Variable>()
        {
            new Variable(Variable.VariableType.VTypeString, "Tag", "GuiElement_"),
            new Variable(Variable.VariableType.VTypeInt, "Position_x", "0"),
            new Variable(Variable.VariableType.VTypeInt, "Position_y", "0"),
            new Variable(Variable.VariableType.VTypeInt, "Width", "100"),
            new Variable(Variable.VariableType.VTypeInt, "Height", "100"),
            new Variable(Variable.VariableType.VTypeBool, "Enabled", "true"),
            new Variable(Variable.VariableType.VTypeBool, "Visible", "true"),
            new Variable(Variable.VariableType.VTypeEnum, "Style", "ALIGN_LEFT", new List<string>()
            {
                "ALIGN_LEFT","ALIGN_RIGHT","ALIGN_CENTER","FILL_CENTER","RELATIVE_PARENT","ABSOLUTE"
            }),
        };

        // templates of gui elements
        // const values from ArtCore
        // create templates for all gui elements
        private static readonly Dictionary<string, List<Variable>> Templates = new Dictionary<string, List<Variable>>()
        {
            {
                "root",
                new List<Variable>()
                {
                    new Variable(Variable.VariableType.VTypeColor, "Pallet_background", "#000000"),
                    new Variable(Variable.VariableType.VTypeColor, "Pallet_background_disable", "#000000"),
                    new Variable(Variable.VariableType.VTypeColor, "Pallet_frame", "#000000"),
                    new Variable(Variable.VariableType.VTypeColor, "Pallet_active", "#000000"),
                    new Variable(Variable.VariableType.VTypeColor, "Pallet_font", "#000000"),
                }
            },
            {
                "Button",
                new List<Variable>()
                {
                    new Variable(Variable.VariableType.VTypeString, "Text", "button")
                }
            },
            {
                "CheckButton",
                new List<Variable>()
                {
                    new Variable(Variable.VariableType.VTypeString, "Text", "check button")
                }
            },
            {
                "Slider",
                new List<Variable>()
                {
                    new Variable(Variable.VariableType.VTypeString, "Text", "slider"),
                    new Variable(Variable.VariableType.VTypeInt, "Minimum", "0"),
                    new Variable(Variable.VariableType.VTypeInt, "Maximum", "100"),
                    new Variable(Variable.VariableType.VTypeInt, "Step", "10"),
                    new Variable(Variable.VariableType.VTypeBool, "ShowValue", "true"),
                }
            },
            {
                "DropDownList",
                new List<Variable>()
                {
                    new Variable(Variable.VariableType.VTypeString, "Text", "button"),
                    new Variable(Variable.VariableType.VTypeString, "Values", "option 1|option 2|option 3")
                }
            },
            {
                "Grid",
                new List<Variable>()
                {
                    new Variable(Variable.VariableType.VTypeInt, "Columns", "3"),
                    new Variable(Variable.VariableType.VTypeInt, "Rows", "3"),
                    new Variable(Variable.VariableType.VTypeInt, "ElementWidth", "64"),
                    new Variable(Variable.VariableType.VTypeInt, "ElementHeight", "64"),
                    new Variable(Variable.VariableType.VTypeInt, "ElementSpacing", "8")
                }
            },
            {
                "Image",
                new List<Variable>()
                {
                    new Variable(Variable.VariableType.VTypeSprite, "Sprite", "null")
                }
            },
            {
                "Label",
                new List<Variable>()
                {
                    new Variable(Variable.VariableType.VTypeString, "Text", "Label")
                }
            },
            {
                "Panel",
                new List<Variable>()
                {
                    // null
                }
            },
            {
                "ProgressBar",
                new List<Variable>()
                {
                    new Variable(Variable.VariableType.VTypeInt, "Minimum", "0"),
                    new Variable(Variable.VariableType.VTypeInt, "Maximum", "100"),
                    new Variable(Variable.VariableType.VTypeInt, "Step", "1"),
                    new Variable(Variable.VariableType.VTypeInt, "height", "32"),
                    new Variable(Variable.VariableType.VTypeEnum, "DrawingStyle", "STEP", new List<string>()
                    {
                        "STEP","SOLID","STEP_BLEND","SOLID_BLEND"
                    })
                }
            }

        };

        public Variable GetVariable(string field)
        {
            foreach (Variable elementVariable in _elementVariables)
            {
                if (elementVariable.Name != field) continue;
                return elementVariable;
            }
            return null;
        }
        public Variable.VariableType GetVariableType(string field)
        {
            foreach (Variable elementVariable in _elementVariables)
            {
                if (elementVariable.Name != field) continue;
                return elementVariable.Type;
            }
            return Variable.VariableType.VTypeNull;
        }

        // get all templates without root
        public static List<string> GetTemplateNames()
        {
            return Templates.Keys.Where(item => !item.Equals("root")).ToList();
        }

        public void SetValue(string field, string value)
        {
            foreach (Variable elementVariable in _elementVariables)
            {
                if (elementVariable.Name != field) continue;
                elementVariable.Default = value;
                return;
            }
        }

        public string GetValue(string field) // think about make it private
        {
            foreach (Variable elementVariable in _elementVariables)
            {
                if (elementVariable.Name != field) continue;
                return elementVariable.Default;
            }
            return string.Empty;
        }

        public List<Variable> GetAllProperties()
        {
            return _elementVariables;
        }

        public void DeleteChildren(string tagToDelete)
        {
            foreach (GuiElement children in _children)
            {
                if (children.GetValue("Tag") != tagToDelete) continue;
                _children.Remove(children);
                return;
            }

        }
        // Get only element children without every children`s child`s
        public List<GuiElement> GetChildren()
        {
            return _children;
        }

        // Get all children`s in collection starting with this node
        public List<GuiElement> GetAllChildren()
        {
            // create return list with self
            List<GuiElement> returnList = new List<GuiElement> { this };
            foreach (GuiElement guiElement in _children)
            {
                returnList.AddRange(guiElement.GetAllChildren());
            }
            return returnList;
        }

        // Get all children`s full names in collection starting with this node
        public List<string> GetAllChildrenNamesList()
        {
            // create return list with self
            List<string> returnList = new List<string> { GetFullName() };
            foreach (GuiElement guiElement in _children)
            {
                returnList.AddRange(guiElement.GetAllChildrenNamesList());
            }
            return returnList;
        }

        // Get all paths of nodes to populate tree view
        public List<string> GetAllChildrenPaths()
        {
            List<string> returnList = new List<string> { GetPath() };
            foreach (GuiElement guiElement in _children)
            {
                returnList.AddRange(guiElement.GetAllChildrenPaths());
            }
            return returnList;
        }

        // get element by tag name
        public GuiElement FindGuiElementByName(string name)
        {
            foreach (GuiElement guiElement in GetAllChildren())
            {
                if (guiElement.GetFullName() == name)
                {
                    return guiElement;
                }
            }

            return null;
        }

        // get element at target path
        public GuiElement FindGuiElement(string path)
        {
            string[] targetPath = path.Split('/');

            switch (targetPath.Length)
            {
                // path is invalid or element not exists
                case 0:
                    return null;

                // check if searched element is this
                case 1:
                    if (targetPath.First() == GetFullName())
                    {
                        return this;
                    };
                    break;

                // need to search in children`s, length is > 2
                default:
                    // recombine path without first element
                    string newPath = string.Join('/', targetPath, 1, targetPath.Length - 1);
                    foreach (GuiElement element in _children)
                    {
                        if (element.GetFullName() != targetPath[1]) continue;
                        // search in child`s, if found return 
                        // in end, founded element will be returned or null if not exists
                        GuiElement search = element.FindGuiElement(newPath);
                        if (search == null) continue;
                        return search;
                    }
                    break;
            }
            return null;
        }

        public string GetFullName()
        {
            return Name + '#' + GetValue("Tag");
        }

        public string GetPath(bool tagOnly = false)
        {
            if (tagOnly)
            {
                // if root element
                if (_parent == null) return GetValue("Tag");
                // else get parent path
                return _parent.GetValue("Tag") + '/' + GetValue("Tag");
            }
            else
            {
                // if root element
                if (_parent == null) return GetFullName();
                // else get parent path
                return _parent.GetPath() + '/' + GetFullName();
            }
        }

        public GuiElement(string name, GuiElement parent)
        {
            SetValue("Tag", "GuiElement_" + (++_uid).ToString());
            Name = name;
            //_path = parent?.GetPath() + GetFullName();
            _parent = parent;
            if (Templates.ContainsKey(name))
            {
                // add custom variables to entire list
                _elementVariables.AddRange(Templates[name]);
            }

        }

        // Add element and get reference to it
        public GuiElement AddChild(string templateName)
        {
            _children.Add(new GuiElement(templateName, this));
            return _children.Last();
        }

        public GuiElement GetParent()
        {
            return _parent;
        }

        // json serialize method not work with parent field because generate infinite loop
        // so after load must by hand set all parents
        public void SetAllParents()
        {
            foreach (GuiElement guiElement in _children)
            {
                guiElement._parent = this;
                guiElement.SetAllParents();
            }
        }

    };
}
