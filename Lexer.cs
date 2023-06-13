class Lexer {
    readonly string Input;
    int Position;
    int ReadPosition;
    char Character;

    public Lexer(string input) {
        Input = input;
        ReadChar();
    }

    public void ReadChar() {
        if(ReadPosition >= Input.Length) {
            Character = (char) 0;
        }else {
            Character = Input[ReadPosition];
        }
        
        Position = ReadPosition;

        ReadPosition++;
    }

    public char PeekChar() {
        if(ReadPosition >= Input.Length) {
            return (char) 0;
        }else {
            return Input[ReadPosition];
        }
    }

    readonly List<char> Whitespace = new() {
        ' ',
        '\t',
        '\n',
        '\r'
    };

    public void SkipWhitespace() {
        while(Whitespace.Contains(Character)) {
            ReadChar();
        }
    }

    public string ReadIdentifier() {
        int start = Position;
        
        while(IsIdentifierReady(Character)) {
            ReadChar();
        }

        return new string(Input[start..Position].AsSpan());
    }

    public string ReadNumber() {
        int start = Position;
        
        while(char.IsDigit(Character)) {
            ReadChar();
        }

        return new string(Input[start..Position].AsSpan());
    }

    public Token NextToken() {
        Token tok = new();

        SkipWhitespace();

        Dictionary<char, TokenType> tokenDict = new() {
            {'=', TokenType.assign},
            {'!', TokenType.not},
            {'<', TokenType.less},
            {'>', TokenType.greater},
            {';', TokenType.semicolon},
            {'(', TokenType.lparen},
            {')', TokenType.rparen},
            {',', TokenType.comma},
            {'+', TokenType.plus},
            {'-', TokenType.minus},
            {'/', TokenType.div},
            {'*', TokenType.mult},
            {'{', TokenType.lbrace},
            {'}', TokenType.rbrace},
            {(char) 0, TokenType.eof}
        };
        if(tokenDict.ContainsKey(Character)) {
            bool wasTwochar = false;
            if(Character == '=' && PeekChar() == '=') {
                tok.TokenType = TokenType.equal;
                tok.Literal = "==";
                
                wasTwochar = true;
            }else if(Character == '!' && PeekChar() == '=') {
                tok.TokenType = TokenType.notequal;
                tok.Literal = "!=";
                
                wasTwochar = true;
            }

            if(wasTwochar) {
                ReadChar();
                ReadChar();
                return tok;
            }

            tok.TokenType = TokenType.illegal;
            if(tokenDict.TryGetValue(Character, out TokenType value)) {
                tok.TokenType = value;
            }
            tok.Literal = Character.ToString();

            ReadChar();
            return tok;
        }else if(IsIdentifierReady(Character)) {
            tok.Literal = ReadIdentifier();
            tok.TokenType = TokenType.ident;
            if(Token.keywords.TryGetValue(tok.Literal, out TokenType tt)) {
                tok.TokenType = tt;
            }
        }else if(char.IsDigit(Character)) {
            tok.Literal = ReadNumber();
            tok.TokenType = TokenType.number; 
        } else {
            tok.TokenType = TokenType.illegal;
            tok.Literal = Character.ToString();
            ReadChar();
        }
        return tok;
    }

    public static bool IsIdentifierReady(char c) {
        return char.IsLetter(c) || c == '_';
    }
}