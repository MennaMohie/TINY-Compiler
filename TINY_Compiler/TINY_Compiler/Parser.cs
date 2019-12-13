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
        Boolean mainFunctionIsPresent = false;
        public Node StartParsing(List<Token> TokenStream)
        {
            this.TokenStream = TokenStream;
            root = new Node("Root Node");
            root.Children.Add(Program());

            if (!mainFunctionIsPresent)
                Errors.Error_List.Add("Your code doesn't contain a main()\n");

            return root;
        }
        Node Program() // Start Predection Rule
        {
            Node program = new Node("Program");
            program.Children.Add(Function_Statements());
            //program.Children.Add(Main_Function());
            MessageBox.Show("Success");
            return program;
        }
        Node Function_Statements()
        {
            if (CheckForNull(Token_Class.Integer) || CheckForNull(Token_Class.String) || CheckForNull(Token_Class.Float))
            {
                Node functionStatements = new Node("Function_Statements");
                functionStatements.Children.Add(Function_Statement());
                functionStatements.Children.Add(Function_Statements());
                return functionStatements;
            }
            return null;
        }
        Node Function_Statement()
        {
            Node functionStatement = new Node("Function_Statement");
            functionStatement.Children.Add(Function_Declaration());
            functionStatement.Children.Add(Function_Body());
            return functionStatement;
        }
        Node Function_Declaration()
        {
            Node functionDeclaration = new Node("Function_Declaration");
            functionDeclaration.Children.Add(Datatype());
            functionDeclaration.Children.Add(Function_Declaration_());
            return functionDeclaration;
        }
        Node Function_Declaration_()
        {
            Node functionDeclaration_ = new Node("Function_Declaration_");
            if (CheckForNull(Token_Class.Main))
            {
                functionDeclaration_.Children.Add(Main_Function());
            }
            else
            {
                functionDeclaration_.Children.Add(FunctionName());
                functionDeclaration_.Children.Add(Match(Token_Class.LParanthesis));
                functionDeclaration_.Children.Add(Parameters());
                functionDeclaration_.Children.Add(Match(Token_Class.RParanthesis));
            }
            return functionDeclaration_;
        }
        Node Function_Body()
        {
            Node functionBody = new Node("Function_Body");
            functionBody.Children.Add(Match(Token_Class.LBrace));
            functionBody.Children.Add(Statements());
            functionBody.Children.Add(Return_Statement());
            functionBody.Children.Add(Match(Token_Class.RBrace));
            return functionBody;
        }
        Node FunctionName()
        {
            Node functionName = new Node("FunctionName");
            functionName.Children.Add(Match(Token_Class.Identifier));
            return functionName;
        }
        Node Main_Function() // main()
        {
            mainFunctionIsPresent = true;
            Node mainFunction = new Node("Main_Function");
            //mainFunction.Children.Add(Datatype());
            mainFunction.Children.Add(Match(Token_Class.Main));
            mainFunction.Children.Add(Match(Token_Class.LParanthesis));
            mainFunction.Children.Add(Match(Token_Class.RParanthesis));
            //mainFunction.Children.Add(Function_Body());
            return mainFunction;
        }
        Node Parameters()
        {
            if (CheckForNull(Token_Class.Integer) || CheckForNull(Token_Class.String) || CheckForNull(Token_Class.Float))
            {
                Node parameters = new Node("Parameters");
                parameters.Children.Add(Parameter());
                parameters.Children.Add(Parameters_());
                return parameters;
            }
            return null;
        }
        Node Parameters_()
        {
            if (CheckForNull(Token_Class.Comma))
            {
                Node parameters_ = new Node("Parameters_");
                parameters_.Children.Add(Match(Token_Class.Comma));
                parameters_.Children.Add(Parameters());
                return parameters_;
            }
            return null;
        }
        Node Parameter()
        {
            Node parameter = new Node("Parameter");
            parameter.Children.Add(Datatype());
            parameter.Children.Add(Match(Token_Class.Identifier));
            return parameter;
        }

        Node Statements()
        {
            if (CheckForNull(Token_Class.Identifier) || CheckForNull(Token_Class.String) ||
                CheckForNull(Token_Class.Integer) || CheckForNull(Token_Class.Float)
                || CheckForNull(Token_Class.If) || CheckForNull(Token_Class.Repeat)
                || CheckForNull(Token_Class.Read) || CheckForNull(Token_Class.Write)
                || CheckForNull(Token_Class.Comment))
            {
                Node statements = new Node("statements");
                statements.Children.Add(Statement());
                return statements;
            }
            return null;
        }
        Node Statement()
        {
            Node statement = new Node("statement");
            if (CheckForNull(Token_Class.If) || CheckForNull(Token_Class.Repeat)
                || CheckForNull(Token_Class.Comment))
            {
                statement.Children.Add(No_Semicolon());
            }
            else
                statement.Children.Add(Ends_With_Semicolon());
            return statement;
        }
        Node Ends_With_Semicolon()
        {
            Node ends_with_semicolon = new Node("ends_with_semicolon");
            if (CheckForNull(Token_Class.Identifier))
            {
                ends_with_semicolon.Children.Add(Assignment_Statement());
            }
            else if (CheckForNull(Token_Class.Integer) || CheckForNull(Token_Class.Float) || CheckForNull(Token_Class.String))
            {
                ends_with_semicolon.Children.Add(Declaration_Statement());
            }
            else if (CheckForNull(Token_Class.Write))
            {
                ends_with_semicolon.Children.Add(Write_Statement());
            }
            else if (CheckForNull(Token_Class.Read))
            {
                ends_with_semicolon.Children.Add(Read_Statement());
            }
            else if (CheckForNull(Token_Class.Identifier) && CheckForNull(Token_Class.LParanthesis))
            {
                ends_with_semicolon.Children.Add(Function_Call());
            }
            return ends_with_semicolon;
        }
        Node No_Semicolon()
        {
            Node no_semicolon = new Node("ends_with_semicolon");
            if (CheckForNull(Token_Class.If))
            {
                no_semicolon.Children.Add(If_Statement());
            }
            else if (CheckForNull(Token_Class.Repeat))
            {
                no_semicolon.Children.Add(Repeat_Statement());
            }
            else
            {
                no_semicolon.Children.Add(Match(Token_Class.Comment));
            }
            return no_semicolon;
        }
        Node Return_Statement()
        {
            Node return_statement = new Node("return_statement");
            return_statement.Children.Add(Match(Token_Class.Return));
            return_statement.Children.Add(Expression());
            return_statement.Children.Add(Match(Token_Class.Semicolon));
            return return_statement;
        }
        Node Function_Call()
        {
            Node function_call = new Node("function_call");
            function_call.Children.Add(Match(Token_Class.Identifier));
            function_call.Children.Add(Parameters_Part());
            return function_call;
        }
        Node Parameters_Part()
        {
            Node parameters_part = new Node("parameters_part");
            parameters_part.Children.Add(Match(Token_Class.LParanthesis));
            parameters_part.Children.Add(Identifiers());
            parameters_part.Children.Add(Match(Token_Class.RParanthesis));
            return parameters_part;
        }
        Node Identifiers()
        {
            Node identifiers = new Node("identifiers");
            if (CheckForNull(Token_Class.Identifier))
            {
                identifiers.Children.Add(Match(Token_Class.Identifier));
                identifiers.Children.Add(Identifiers_());
                return identifiers;
            }
            return null;
        }
        Node Identifiers_()
        {
            Node identifiers_ = new Node("identifiers_");
            if (CheckForNull(Token_Class.Identifier))
            {
                identifiers_.Children.Add(Match(Token_Class.Comma));
                identifiers_.Children.Add(Match(Token_Class.Identifier));
                return identifiers_;
            }
            return null;
        }
        Node Repeat_Statement()
        {
            Node repeat_statement = new Node("repeat_statement");
            repeat_statement.Children.Add(Match(Token_Class.Repeat));
            repeat_statement.Children.Add(Statements());
            repeat_statement.Children.Add(Match(Token_Class.Until));
            repeat_statement.Children.Add(Condition_Statement());
            return repeat_statement;
        }
        Node Condition_Statement()
        {
            Node condition_statement = new Node("condition_statement");
            condition_statement.Children.Add(Condition());
            condition_statement.Children.Add(Condition_Statement_());
            return condition_statement;
        }
        Node Condition_Statement_()
        {
            if (CheckForNull(Token_Class.Or) || CheckForNull(Token_Class.And))
            {
                Node condition_statement_ = new Node("condition_statement_");
                condition_statement_.Children.Add(Boolean_Operator());
                condition_statement_.Children.Add(Condition_Statement());
                return condition_statement_;
            }
            return null;
        }
        Node Condition()
        {
            Node condition = new Node("condition");
            condition.Children.Add(Match(Token_Class.Identifier));
            condition.Children.Add(Condition_Operator());
            condition.Children.Add(Term());
            return condition;
        }
        Node Term()
        {
            Node term = new Node("term");
            if (CheckForNull(Token_Class.Identifier))
            {
                term.Children.Add(Match(Token_Class.Identifier));
                term.Children.Add(Function_Call());
            }
            else if (CheckForNull(Token_Class.Integer))
                term.Children.Add(Match(Token_Class.Integer));
            else
                term.Children.Add(Match(Token_Class.Float));
            return term;
        }
        Node Term_Factoring()
        {
            if (CheckForNull(Token_Class.LParanthesis))
            {
                Node term_factoring = new Node("term_factoring");
                term_factoring.Children.Add(Parameters_Part());
                return term_factoring;
            }
            else
                return null;
        }
        Node Boolean_Operator()
        {
            Node boolean_operator = new Node("boolean_operator");
            if (CheckForNull(Token_Class.Or))
            {
                boolean_operator.Children.Add(Match(Token_Class.Or));
                boolean_operator.Children.Add(Match(Token_Class.Or));
            }
            else
            {
                boolean_operator.Children.Add(Match(Token_Class.And));
                boolean_operator.Children.Add(Match(Token_Class.And));
            }
            return boolean_operator;
        }
        Node Condition_Operator()
        {
            Node condition_operator = new Node("condition_operator");
            if (CheckForNull(Token_Class.EqualOp))
                condition_operator.Children.Add(Match(Token_Class.EqualOp));
            if (CheckForNull(Token_Class.GreaterThanOp))
                condition_operator.Children.Add(Match(Token_Class.GreaterThanOp));
            if (CheckForNull(Token_Class.LessThanOp))
                condition_operator.Children.Add(Match(Token_Class.LessThanOp));
            else
                condition_operator.Children.Add(Match(Token_Class.NotEqualOp));
            return condition_operator;
        }
        Node Write_Statement()
        {
            Node write_statement = new Node("write_statement");
            write_statement.Children.Add(Match(Token_Class.Write));
            write_statement.Children.Add(Next());
            return write_statement;
        }
        Node Next()
        {
            Node next = new Node("next");
            if (CheckForNull(Token_Class.Endl))
                next.Children.Add(Match(Token_Class.Endl));
            else
                next.Children.Add(Expression());
            return next;
        }
        Node Read_Statement()
        {
            Node read_statement = new Node("read_statement");
            read_statement.Children.Add(Match(Token_Class.Read));
            read_statement.Children.Add(Match(Token_Class.Identifier));
            return read_statement;
        }
        Node Datatype()
        {
            Node datatype = new Node("datatype");
            if (CheckForNull(Token_Class.Integer))
                datatype.Children.Add(Match(Token_Class.Integer));
            if (CheckForNull(Token_Class.Float))
                datatype.Children.Add(Match(Token_Class.Float));
            if (CheckForNull(Token_Class.String))
                datatype.Children.Add(Match(Token_Class.String));
            return datatype;
        }
        Node Declaration_Statement()
        {
            Node seclaration_statement = new Node("seclaration_statement");
            seclaration_statement.Children.Add(Datatype());
            seclaration_statement.Children.Add(Declaration_Details());
            return seclaration_statement;
        }
        Node Declaration_Details()
        {
            Node declaration_details = new Node("declaration_details");
            declaration_details.Children.Add(Declaration_Detail());
            declaration_details.Children.Add(Declaration_Details_());
            return declaration_details;
        }
        Node Declaration_Details_()
        {
            if (CheckForNull(Token_Class.Comma))
            {
                Node declaration_details_ = new Node("declaration_details_");
                declaration_details_.Children.Add(Match(Token_Class.Comma));
                declaration_details_.Children.Add(Declaration_Details());
                return declaration_details_;
            }
            return null;
        }
        Node Declaration_Detail()
        {
            Node declaration_detail = new Node("declaration_detail");
            declaration_detail.Children.Add(Match(Token_Class.Identifier));
            declaration_detail.Children.Add(Other_Detail());
            return declaration_detail;
        }
        Node Other_Detail()
        {
            Node other_detail = new Node("other_detail");
            other_detail.Children.Add(Match(Token_Class.Assignment));
            other_detail.Children.Add(Expression());
            return other_detail;
        }

        Node Assignment_Statement()
        {
            //Assignment_Statement ➔ identifier:= Expression
            Node assignmentStatement = new Node("assignmentStatement");
            assignmentStatement.Children.Add(Match(Token_Class.Identifier));
            assignmentStatement.Children.Add(Match(Token_Class.Assignment));
            assignmentStatement.Children.Add(Expression());
            return assignmentStatement;
        }
        Node Expression()
        {
            //Expression ➔ String | Term | Equation
            Node expression = new Node("expression");
            if (CheckForNull(Token_Class.StringLiteral))
                expression.Children.Add(Match(Token_Class.StringLiteral));
            else if (CheckForNull(Token_Class.LBrace))
                expression.Children.Add(Equation());
            else
                expression.Children.Add(Term());
            return expression;
        }
        Node Equation()
        {
            //Equation ➔ TermEq Equation_
            Node equation = new Node("equation");
            equation.Children.Add(TermEq());
            equation.Children.Add(Equation_());
            return equation;
        }
        Node Equation_()
        {
            //Equation_ ➔ AddTerm Equation_ | ε
            Node equation_ = new Node("equation_");
            if (CheckForNull(Token_Class.PlusOp) || CheckForNull(Token_Class.MinusOp))
            {
                equation_.Children.Add(AddTerm());
                equation_.Children.Add(Equation_());
                return equation_;
            }
            return null;
        }
        Node AddTerm()
        {
            //AddTerm ➔ AddOperation TermEq
            Node addTerm = new Node("addTerm");
            addTerm.Children.Add(AddOperation());
            addTerm.Children.Add(TermEq());
            return addTerm;
        }
        Node TermEq()
        {
            //TermEq ➔ Factor TermEq_ 
            Node termEq = new Node("termEq");
            termEq.Children.Add(Factor());
            termEq.Children.Add(TermEq_());
            return termEq;
        }
        Node TermEq_()
        {
            //TermEq_ ➔ MulTerm TermEq_ | ε
            Node termEq_ = new Node("termEq_");
            if(CheckForNull(Token_Class.MultiplyOp) || CheckForNull(Token_Class.DivideOp))
            {
                termEq_.Children.Add(MulTerm());
                termEq_.Children.Add(TermEq_());
                return termEq_;
            }
            return null;
        }
        Node MulTerm()
        {
            //MulTerm ➔ MulOperation Factor
            Node mulTerm = new Node("mulTerm");
            mulTerm.Children.Add(MulOperation());
            mulTerm.Children.Add(Factor());
            return mulTerm;
        }
        Node Factor()
        {
            //Factor ➔ (Equation) | Term
            Node factor = new Node("factor");
            if (CheckForNull(Token_Class.LBrace))
            {
                factor.Children.Add(Match(Token_Class.LBrace));
                factor.Children.Add(Equation());
                factor.Children.Add(Match(Token_Class.RBrace));
            }
            else
            {
                factor.Children.Add(Term());
            }
            return factor;
        }
        Node AddOperation()
        {
            //AddOperation ➔ + | -
            Node addOperation = new Node("addOperation");
            if (CheckForNull(Token_Class.PlusOp))
                addOperation.Children.Add(Match(Token_Class.PlusOp));
            else
                addOperation.Children.Add(Match(Token_Class.MinusOp));
            return addOperation;
        }
        Node MulOperation()
        {
            //MulOperation ➔ * | /
            Node mulOperation = new Node("mulOperation");
            if (CheckForNull(Token_Class.MultiplyOp))
                mulOperation.Children.Add(Match(Token_Class.MultiplyOp));
            else
                mulOperation.Children.Add(Match(Token_Class.DivideOp));
            return mulOperation;
        }
        Node If_Statement()
        {
            //If_Statement ➔ if Condition_Statement then Statements ElseClause
            Node ifStatement = new Node("ifStatement");
            ifStatement.Children.Add(Match(Token_Class.If));
            ifStatement.Children.Add(Condition_Statement());
            ifStatement.Children.Add(Match(Token_Class.Then));
            ifStatement.Children.Add(Statements());
            ifStatement.Children.Add(ElseClause());
            return ifStatement;
        }
        Node ElseClause()
        {
            //ElseClause ➔ Else_If_Statment | Else_Statment | end
            Node elseClause = new Node("elseClause");
            if(CheckForNull(Token_Class.Elseif))
                elseClause.Children.Add(Else_If_Statment());
            else if(CheckForNull(Token_Class.Else))
                elseClause.Children.Add(Else_Statment()); 
            else
                elseClause.Children.Add(Match(Token_Class.End));
            return elseClause;
        }

        Node Else_If_Statment()
        {
            //Else_If_Statment ➔ elseif Condition_Statement then Statements ElseClause
            Node elseIfStatment = new Node("elseIfStatment");
            elseIfStatment.Children.Add(Match(Token_Class.Elseif));
            elseIfStatment.Children.Add(Condition_Statement());
            elseIfStatment.Children.Add(Match(Token_Class.Then));
            elseIfStatment.Children.Add(Statements());
            elseIfStatment.Children.Add(ElseClause());
            return elseIfStatment;
        }

        Node Else_Statment()
        {
            //Else_Statment ➔ else Statements end
            Node elseStatment = new Node("elseStatment");
            elseStatment.Children.Add(Match(Token_Class.Else));
            elseStatment.Children.Add(Statements());
            elseStatment.Children.Add(Match(Token_Class.End));
            return elseStatment;
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
                    " found\r\n");
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
