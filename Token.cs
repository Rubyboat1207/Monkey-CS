public enum TokenType {
    // weird
    illegal = 0,
    eof = 1,

    //types
    ident = 200,
    number = 201,

    //assignment
    assign = 300,

    //math & logic
    plus = 400,
    minus = 401,
    mult = 402,
    div = 403,
    not = 404,
    less = 405,
    greater = 406,
    equal = 407,
    notequal = 408,

    //seperators
    comma = 500,
    semicolon = 501,

    //containers
    lparen = 600,
    rparen = 601,
    lbrace = 602,
    rbrace = 603,

    //langauge
    function = 700,
    val = 701,
    var = 702,
    booleanTrue = 703,
    booleanFalse = 704,
    _if = 705,
    _else = 706,
    _return = 707,
}

public struct Token {
    public static Dictionary<string, TokenType> keywords = new() {
        {"var", TokenType.var},
        {"val", TokenType.val},
        {"fn", TokenType.function},
        {"true", TokenType.booleanTrue},
        {"false", TokenType.booleanFalse},
        {"if", TokenType._if},
        {"else", TokenType._else},
        {"return", TokenType._return},
    };

    public TokenType TokenType;
    public string? Literal;

    public Token() { }

    public Token(TokenType tokenType) {
        TokenType = tokenType;
    }

    public Token(TokenType tokenType, string literal) {
        TokenType = tokenType;
        Literal = literal;
    }

    // override object.Equals
    public override bool Equals(object obj)
    {
        //
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //
        
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }


        
        return TokenType == ((Token) obj).TokenType && Literal == ((Token) obj).Literal;
    }
    
    // override object.GetHashCode
    public override int GetHashCode()
    {
        // first 15 bits is the token type
        // 16th bit is whether or not it has a literal
        // last 16 bits is the hash of the literal

        int ret = (int) TokenType & 0x7FFF;
        if(Literal != null) {
            ret += 16384;
            int lithash = Literal.GetHashCode();
            ret += (lithash & 0xFFFF) << 16; // truncation might not be a problem
        }

        return ret;
    }

    public static bool operator ==(Token left, Token right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Token left, Token right)
    {
        return !(left == right);
    }

    public override string ToString()
    {
        return TokenType.ToString() + $" | ({Literal})";
    }
}

