using System;
using TYAPK;

List<Terminal> terminals = new List<Terminal> { new Terminal("0"), new Terminal("1") };
List<NonTerminal> nonTerminals = new List<NonTerminal> { new NonTerminal("Number"), new NonTerminal("NaturalNumber"), new NonTerminal("Exception") };
List<GrammaticRule> rules = new List<GrammaticRule> { new GrammaticRule("<NaturalNumber>-><Number>0101<NaturalNumber>"), new GrammaticRule("<NaturalNumber>->0110"),
new GrammaticRule("<Exception>-><Exception>"), new GrammaticRule("<Number>-><Exception>"), new GrammaticRule("<Exception>-><Number>"), new GrammaticRule("<Exception>-><Number>10")};
NonTerminal axiom = new NonTerminal("Exception");
Grammar grammar = new Grammar(nonTerminals, terminals, rules, axiom);
Console.WriteLine(grammar);
Console.WriteLine();
Console.WriteLine(grammar.EliminateUnattainableSymbols());

public static class GrammarExtentions
{
    public static Grammar? EliminateUnattainableSymbols(this Grammar inputGrammar)
    {
        if (inputGrammar == null) { return null; } //проверка на null
        var axiom = inputGrammar.axiom; //создаем копию аксиомы для удобства
        List<NonTerminal> attainableNonTerminals = new(); //массив нетерминалов новой грамматики
        List<Terminal> attainableTerminals = new(); //массив терминалов новой грамматики
        Queue<NonTerminal> queue = new(); //очередь обработки нетерминалов
        queue.Enqueue(axiom); //начинаем с обработки аксиомы
        attainableNonTerminals.Add(axiom);
        while (queue.TryPeek(out _)) //пока в очереди есть необработанные вершины делаем цикл
        {
            NonTerminal cur = queue.Dequeue(); //достаем из очереди элемент     
            foreach (var gr in inputGrammar.grammaticRules) //перебираем все правила в старой грамматике
            {
                if (gr.correctRule && cur.Equals(gr.leftExpression)) //если правило "корректное" и в его левой части содержится рассматриваемый нетерминал
                {
                    foreach (var sym in gr.rightExpression) //то всю его необработанную правую часть записываем в новую грамматику в соответствующий массив и добавляем в очередь
                    {
                        if (sym is NonTerminal)
                        {
                            if (!attainableNonTerminals.Contains(sym))
                            {
                                queue.Enqueue((NonTerminal)sym);
                                attainableNonTerminals.Add((NonTerminal)sym);
                            }
                        }
                        else
                        {
                            if (!attainableTerminals.Contains(sym))
                                attainableTerminals.Add((Terminal)sym);
                        }
                    }
                }
            }
        }
        return new Grammar(attainableNonTerminals, attainableTerminals, inputGrammar.grammaticRules, axiom); //возвращаем новую грамматику
    }
}
