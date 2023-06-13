public struct Case {
    List<Token> Expected;    
    public string Input;
    public string Name;

    public readonly bool Test(List<Token> evaluated) {
        for(int i = 0; i < evaluated.Count; i++) {
            if(i > Expected.Count - 1) {
                Console.WriteLine($"Failed at: {i} -> thought it ended, got {evaluated[i]}");
                return false;
            }
            if(Expected[i] != evaluated[i]) {
                Console.WriteLine($"Failed at: {i} -> Expected {Expected[i]}, got {evaluated[i]}");
                return false;
            }
        }
        return true;
    }

    public Case(string name, string input, List<Token> expected) {
        Name = name;
        Input = input;
        Expected = expected;
    }
}

public class Testing {
    public static void Test() {
        List<Case> cases = new();
        Lexer subject;

        cases.Add(new Case("Symbols", "=+-*/(){},<>;", new() {
            new Token(TokenType.assign, "="),
            new Token(TokenType.plus, "+"),
            new Token(TokenType.minus, "-"),
            new Token(TokenType.mult, "*"),
            new Token(TokenType.div, "/"),
            new Token(TokenType.lparen, "("),
            new Token(TokenType.rparen, ")"),
            new Token(TokenType.lbrace, "{"),
            new Token(TokenType.rbrace, "}"),
            new Token(TokenType.comma, ","),
            new Token(TokenType.less, "<"),
            new Token(TokenType.greater, ">"),
            new Token(TokenType.semicolon, ";"),
            new Token(TokenType.eof, ((char) 0).ToString()),
        }));

        cases.Add(new Case("variable", "var ten = 10;", new() {
            new Token(TokenType.var, "var"),
            new Token(TokenType.ident, "ten"),
            new Token(TokenType.assign, "="),
            new Token(TokenType.number, "10"),
            new Token(TokenType.semicolon, ";"),
            new Token(TokenType.eof, ((char) 0).ToString()),
        }));

        cases.Add(new Case("value", "val ten = 10;", new() {
            new Token(TokenType.val, "val"),
            new Token(TokenType.ident, "ten"),
            new Token(TokenType.assign, "="),
            new Token(TokenType.number, "10"),
            new Token(TokenType.semicolon, ";"),
            new Token(TokenType.eof, ((char) 0).ToString()),
        }));

        cases.Add(new Case("equality", "ten == 10;", new() {
            new Token(TokenType.ident, "ten"),
            new Token(TokenType.equal, "=="),
            new Token(TokenType.number, "10"),
            new Token(TokenType.semicolon, ";"),
            new Token(TokenType.eof, ((char) 0).ToString()),
        }));

        cases.Add(new Case("inequality", "ten != 10;", new() {
            new Token(TokenType.ident, "ten"),
            new Token(TokenType.notequal, "!="),
            new Token(TokenType.number, "10"),
            new Token(TokenType.semicolon, ";"),
            new Token(TokenType.eof, ((char) 0).ToString()),
        }));

        cases.Add(new Case("not val", "!ten;", new() {
            new Token(TokenType.not, "!"),
            new Token(TokenType.ident, "ten"),
            new Token(TokenType.semicolon, ";"),
            new Token(TokenType.eof, ((char) 0).ToString()),
        }));

        cases.Add(new Case("function", "var add = fn(x, y) {x + y;};", new() {
            new Token(TokenType.var, "var"),
            new Token(TokenType.ident, "add"),
            new Token(TokenType.assign, "="),
            new Token(TokenType.function, "fn"),
            new Token(TokenType.lparen, "("),
            new Token(TokenType.ident, "x"),
            new Token(TokenType.comma, ","),
            new Token(TokenType.ident, "y"),
            new Token(TokenType.rparen, ")"),
            new Token(TokenType.lbrace, "{"),
            new Token(TokenType.ident, "x"),
            new Token(TokenType.plus, "+"),
            new Token(TokenType.ident, "y"),
            new Token(TokenType.semicolon, ";"),
            new Token(TokenType.rbrace, "}"),
            new Token(TokenType.semicolon, ";"),
            new Token(TokenType.eof, ((char) 0).ToString()),
        }));

        cases.Add(new Case("Code", 
          @"var five = 5;
            var ten = 10;
            var add = fn(x, y) {
                x + y;
            };
            var result = add(five, ten);", new() {
            new Token(TokenType.var, "var"),
            new Token(TokenType.ident, "five"),
            new Token(TokenType.assign, "="),
            new Token(TokenType.number, "5"),
            new Token(TokenType.semicolon, ";"),
            new Token(TokenType.var, "var"),
            new Token(TokenType.ident, "ten"),
            new Token(TokenType.assign, "="),
            new Token(TokenType.number, "10"),
            new Token(TokenType.semicolon, ";"),
            new Token(TokenType.var, "var"),
            new Token(TokenType.ident, "add"),
            new Token(TokenType.assign, "="),
            new Token(TokenType.function, "fn"),
            new Token(TokenType.lparen, "("),
            new Token(TokenType.ident, "x"),
            new Token(TokenType.comma, ","),
            new Token(TokenType.ident, "y"),
            new Token(TokenType.rparen, ")"),
            new Token(TokenType.lbrace, "{"),
            new Token(TokenType.ident, "x"),
            new Token(TokenType.plus, "+"),
            new Token(TokenType.ident, "y"),
            new Token(TokenType.semicolon, ";"),
            new Token(TokenType.rbrace, "}"),
            new Token(TokenType.semicolon, ";"),
            new Token(TokenType.var, "var"),
            new Token(TokenType.ident, "result"),
            new Token(TokenType.assign, "="),
            new Token(TokenType.ident, "add"),
            new Token(TokenType.lparen, "("),
            new Token(TokenType.ident, "five"),
            new Token(TokenType.comma, ","),
            new Token(TokenType.ident, "ten"),
            new Token(TokenType.rparen, ")"),
            new Token(TokenType.semicolon, ";"),
            new Token(TokenType.eof, ((char) 0).ToString()),
        }));

        cases.ForEach(c => {
            subject = new(c.Input);

            List<Token> tokens = new();

            while(tokens.Count == 0 || tokens[^1].TokenType != TokenType.eof) {
                tokens.Add(subject.NextToken());
            }

            bool results = c.Test(tokens);

            if(results) {
                Console.WriteLine($"Test: {c.Name} has passed 🎉");
            }else {
                Console.WriteLine($"Test: {c.Name} has failed. Debug: ");
                tokens.ForEach(t => Console.WriteLine(t.TokenType));
            }
        });
    }
}