using System.Linq;
using System.Collections.Generic;
namespace task01{
public static class StringExtensions
{
    public static bool IsPalindrome(this string input)
    {
        if (input == null) return false;
        if (input == "") return false;

        string lowerCase = input.ToLower();
        Stack<char> charfromstack = new Stack<char>();
        Queue<char> charfromqueue = new Queue<char>();

        foreach (var s in lowerCase) 
        {
            if ((!Char.IsPunctuation(s)) && (!Char.IsWhiteSpace(s))) { charfromstack.Push(s); charfromqueue.Enqueue(s); }}
        
        int len = charfromqueue.Count;
        if (len == 0) return false;
        for (int i = 0; i< len; i++) 
        { if (!(charfromstack.Pop() == charfromqueue.Dequeue())) {return false; }}
        return true;
    }
            


}
}