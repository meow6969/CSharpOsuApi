// ReSharper disable MemberCanBePrivate.Global

using System.Reflection;

namespace CSharpOsuApi.Models;

public class Scopes : OsuClass
{
    public abstract class BaseScope : OsuClass
    {
        public bool All = false;
    }
    
    public class ChatScopes : BaseScope
    {
        public bool Read = false;
        public bool Write = false;
        public bool WriteManage = false;
    }

    public class ForumScopes : BaseScope
    {
        public bool Write = false;
    }

    public class FriendsScopes : BaseScope
    {
        public bool Read = false;
    }
    
    // all the scopes in 1 thing except for delegate cause like this for testing stuff
    // identify+public+chat.read+chat.write+chat.write_manage+forum.write+friends.read
    public static string ScopeBuilder(params string[] scopes)
    {
        List<string> individualScopes = [];
        string finalScopes = "";
        foreach (string potentialMultipleScopes in scopes)
        {
            foreach (string individualScope in potentialMultipleScopes.Split("+"))
            {
                if (individualScopes.Contains(individualScope)) continue;
                if (individualScope.Trim() == "") continue;
                individualScopes.Add(individualScope);
                finalScopes += individualScope + "+";
            }
        }   
        // - 2 here because string.Length
        return finalScopes.Substring(0, finalScopes.Length - 1);
    }

    public override string ToString()
    {
        string finalScopes = "";
        
        foreach (FieldInfo fieldInfo in GetType().GetFields())
        {
            if (fieldInfo.FieldType.IsSubclassOf(typeof(BaseScope)))
            {
                string subClassName = GetProperScopeName(fieldInfo.Name) + ".";
                bool subClassAll = All;
                if (!subClassAll) subClassAll = ((BaseScope)fieldInfo.GetValue(this)!).All;
                foreach (FieldInfo subClassFieldInfo in fieldInfo.FieldType.GetFields())
                {
                    if (!(bool)subClassFieldInfo.GetValue(fieldInfo.GetValue(this))! && !subClassAll) continue;
                    if (subClassFieldInfo.Name == "All") continue;
                    finalScopes += subClassName + GetProperScopeName(subClassFieldInfo.Name) + "+";
                }
            }
            else
            {
                if (!All && !(bool)fieldInfo.GetValue(this)!) continue;
                if (fieldInfo.Name == "All") continue;
                finalScopes += GetProperScopeName(fieldInfo.Name) + "+";
            }
        }

        return finalScopes[..^1];
    }

    private static string GetProperScopeName(string theScope)
    {
        string scopeName = "";
        for (int i = 0; i < theScope.Length; i++)
        {
            if (i == 0)
            {
                scopeName += theScope[i];
                continue;
            }
            if (char.IsUpper(theScope[i]))
            {
                scopeName += $"_{theScope[i]}";
                continue;
            }

            scopeName += theScope[i];
        }

        scopeName = scopeName.ToLower();

        return scopeName;
    }
    
    // private static string GetProperScopeName(FieldInfo theScope, FieldInfo theBase)
    // {
    //     string scopeName = "";
    //     if (theBase.FieldType.BaseType!.IsSubclassOf(typeof(BaseScope)))
    //     {
    //         scopeName += $"{GetProperScopeName(theBase.Name)}.";
    //     }
    //     else
    //     {
    //         throw new Exception();
    //     }
    //
    //     return scopeName;
    // }
    
    public bool Delegate = false;
    public bool Identify = false;
    public bool Public = false;
    public ChatScopes Chat = new ChatScopes();
    public ForumScopes Forum = new ForumScopes();
    public FriendsScopes Friends = new FriendsScopes();
    
    public bool All = false;
    
    // public const string Delegate = "delegate";
    // public const string Identify = "identify";
    // public const string Public = "public";
    //     
    // public abstract class Chat
    // {
    //     public const string Read = "chat.read";
    //     public const string Write = "chat.write";
    //     public const string WriteManage = "chat.write_manage";
    //     // ReSharper disable once MemberHidesStaticFromOuterClass
    //     public const string All = $"{Read}+{Write}+{WriteManage}";
    // }
    //
    // public abstract class Forum
    // {
    //     public const string Write = "forum.write";
    //     // ReSharper disable once MemberHidesStaticFromOuterClass
    //     public const string All = $"{Write}";
    // }
    //
    // public abstract class Friends
    // {
    //     public const string Read = "friends.read";
    //     // ReSharper disable once MemberHidesStaticFromOuterClass
    //     public const string All = $"{Read}";
    // }
    //
    // public const string All = $"{Chat.All}+{Forum.All}+{Friends.All}+{Delegate}+{Identify}+{Public}";
}
