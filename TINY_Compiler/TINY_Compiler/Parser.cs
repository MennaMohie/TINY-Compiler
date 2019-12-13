using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TINY_Compiler
{
    public class Node
    {
        public List<Node> Children = new List<Node>();
        
        public string Name; //Esm el-Node
        public Node(string N)
        {
            this.Name = N;
        }
    }
    public class Parser
    {
        int InputPointer = 0; // Boy2af 3la el-next token, works on TokenStream List
        List<Token> TokenStream;
        public  Node root;
        
        public Node StartParsing(List<Token> TokenStream)
        {
            this.TokenStream = TokenStream;
            root = new Node("Root Node");
            root.Children.Add(Program());
            return root;
        }
        Node Program() // Start Predection Rule
        {
            Node program = new Node("Program");
            /*
            program.Children.Add(Header());
            program.Children.Add(DeclSec());
            program.Children.Add(Block());
            program.Children.Add(match(Token_Class.Dot)); // match takes the expected Token
            MessageBox.Show("Success");
            */
            return program;
        }
        Node Main_Function()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node Function_Statement()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node Function_Declaration()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node FunctionName()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node Parameters()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node Parameters_()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node Parameter()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node Function_Body()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }

        Node Statements()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node Statement()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node Return_Statement()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node Function_Call()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node Identifiers()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node Identifiers_()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node Repeat_Statement()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node Condition_Statement()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node Condition_Statement_()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node Condition()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node Term()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node Write_Statement()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node Next()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node Read_Statement()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node Datatype()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node Declaration_Statement()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }

        Node Assignment_Statement()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node Expression()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node Equation()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node Equation_()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node AddTerm()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node TermEq()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node TermEq_()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node MulTerm()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node Factor()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node AddOperation()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node MulOperation()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node If_Statement()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node ElseClause()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node Else_If_Statment()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }
        Node Else_Statment()
        {
            Node place_holder = new Node("place_holder");
            return place_holder;
        }


        public bool CheckForNull(Token_Class token)
        {
            if (TokenStream[InputPointer].token_type == token)
                return true;
            return false;
        }

        public Node Match(Token_Class ExpectedToken)
        {
            
            if (ExpectedToken == TokenStream[InputPointer].token_type)
            {
                InputPointer++;
                Node newNode = new Node(ExpectedToken.ToString());

                return newNode;
                    
            }

            else
            {
                Errors.Error_List.Add("Parsing Error: Expected " 
                    + ExpectedToken.ToString() + " and " + 
                    TokenStream[InputPointer].token_type.ToString() + 
                    "  found\r\n");
                InputPointer++;
                return null;
            }
        }

        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.Children.Count == 0)
                return tree;
            foreach (Node child in root.Children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
    }
}
