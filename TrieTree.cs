using System;
using System.Collections.Generic;
using System.IO;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {

            var tree = new TrieTree();
            try {
                foreach (string line in File.ReadLines(@"./words.txt")){
                    tree.AddWord(line.Trim());
                }
            } catch (Exception e) {
                /*  Just catch all exceptions. If this were a real app, I should
                    try to catch different kinds of exceptions and handle them
                    appropriately, for example, notify the user that a file doesn't
                    exist, rather than just print the exception like below.
                */
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Enter a word and press enter.\nIt will say whether that's a word");
            while (true){
                var input = Console.ReadLine().Trim();
                if (tree.CheckWord(input)){
                    Console.WriteLine("That's a word!");
                } else {
                    Console.WriteLine("That's not a word!");
                }
            }
        }
    }

    public class TrieTree
    {
        /* The Trie Tree is a datastructure to quickly check whether
        a given word exists. The tree's depth is determined by the max length of word
        stored in it. */

        /* Dictionary to keep track of child nodes. Each level of this tree
        corresponds to the Nth letter of a word.*/
        private IDictionary<char, TrieTree> nodes = new Dictionary<char, TrieTree>();
        /* "valid" indicates whether the word whose letters match the path
        through the tree is a valid word */
        private bool valid = false;

        /* We will use the helper function recursive pattern. Call the
        private, general case, function in this public method.*/
        public bool AddWord(string word){
            return AddWordHelper(word: word, idx: 0);
        }

        public bool CheckWord(string word){
            return CheckWordHelper(word: word, idx: 0);
        }

        private bool AddWordHelper(string word, int idx){
            if (word.Length == idx){
                //Store whether this node was previously valid.
                //This is so we can return True when a new word is added,
                //and False when a word already existed.
                bool prev_valid = valid;
                valid = true;
                return !prev_valid;
            }

            char letter = word[idx];
            var containsKey = nodes.ContainsKey(letter);

            TrieTree node;
            if (!containsKey){
                node = new TrieTree();
                this.nodes[letter] = node;
            } else {
                node = this.nodes[letter];
            }

            return node.AddWordHelper(word, idx+1);
        }

        private bool CheckWordHelper(string word, int idx){
            if (word.Length == idx){
                return valid;
            }

            char letter = word[idx];
            var containsKey = nodes.ContainsKey(letter);

            if (!containsKey){
                return false;
            }

            return nodes[letter].CheckWordHelper(word, idx+1);
        }
    }
}
