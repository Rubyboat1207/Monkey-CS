class Repl {
    public void Start() {
        string? input = Console.ReadLine();
        while(input != null && input != "exit") {

            Lexer lex = new(input);

            List<Token> tokens = new();

            while(tokens.Count == 0 || tokens[^1].TokenType != TokenType.eof) {
                tokens.Add(lex.NextToken());
            }

            tokens.ForEach(t => Console.WriteLine(t));
            input = Console.ReadLine();
        }
    }
}