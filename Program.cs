using System;
using System.Collections.Generic;
using TYAPK;

List<Terminal> terminals = new List<Terminal> { new Terminal("0"), new Terminal("1")};
List<NonTerminal> nonTerminals = new List<NonTerminal> { new NonTerminal("Number"), new NonTerminal("NaturalNumber")};
List<GrammaticRule> rules = new List<GrammaticRule> { new GrammaticRule("<NaturalNumber>-><Number>"), new GrammaticRule("<Number><NaturalNumber>->0110")};
NonTerminal axiom = new NonTerminal("NaturalNumber");
Grammar grammar = new Grammar(nonTerminals, terminals, rules, axiom);
Console.WriteLine(grammar.isEmptyLanguage());