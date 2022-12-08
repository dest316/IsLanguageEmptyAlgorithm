using System;
using System.Collections.Generic;
using TYAPK;

List<Terminal> terminals = new List<Terminal> { new Terminal("0"), new Terminal("1")};
List<NonTerminal> nonTerminals = new List<NonTerminal> { new NonTerminal("Number"), new NonTerminal("NaturalNumber")};
List<GrammaticRule> rules = new List<GrammaticRule> { new GrammaticRule("<NaturalNumber>-><Number>0101<NaturalNumber>"), new GrammaticRule("<NaturalNumber>->0110"), new GrammaticRule("<Number>-><NaturalNumber>121")};
NonTerminal axiom = new NonTerminal("Number");
Grammar grammar = new Grammar(nonTerminals, terminals, rules, axiom);
Console.WriteLine(grammar.isEmptyLanguage());