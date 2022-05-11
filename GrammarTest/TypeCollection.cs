using System;
using System.Collections.Generic;

namespace GrammarTest
{
    public class TypeCollection
    {
        public class Type
        {
            public string TypeName { get; set; }
            // The following line declares <identifier, identifierType> pairs.
            public Dictionary<string, string> Members { get; set; }
            public int NestedBlockLevel { get; set; }

            public Type()
            {
            }

            public Type(string typeName)
            {
                TypeName = typeName;
                Members = null;
                NestedBlockLevel = 0;
            }
        }

        public List<Type> Types;

        public TypeCollection()
        {
            Types = new List<Type>();
            Types.Add(new Type("int"));
            //and others
        }

        public void AddType(Type type)
        {
            Types.Add(type);
        }

        public string GenType()
        {
            // Instantiate random number generator using system-supplied value as seed.
            Random rand = new Random();
            // Generate and display a random integer between 0 and listCount.
            int index = rand.Next(Types.Count);
            // ar trebui sa iau in calcul si blocklevel
            return Types[index].TypeName;
        }

        public string GenIdentifier() //ar trebui sa generez identificatorul in functie de tip?
        {
            string identifiers = "stuv";
            Random rand = new Random();
            int index = rand.Next(identifiers.Length);
            return identifiers[index].ToString();
        }

        public string GenPointer()
        {
            string pointers = "pqr";
            Random rand = new Random();
            int index = rand.Next(pointers.Length);
            // dereferencingOperator&identifier
            return $"*{pointers[index].ToString()}";
        }

        public string GenDecl()
        {
            string type = GenType();
            if (type == "int")
            {
                string identifier = GenIdentifier();
                //put into symbol table
                return $"{type} {identifier};";
            }
            else
            {
                string pointer = GenPointer();
                //put into symbol table
                return $"{type} {pointer};";
            }
        }
    }
}
