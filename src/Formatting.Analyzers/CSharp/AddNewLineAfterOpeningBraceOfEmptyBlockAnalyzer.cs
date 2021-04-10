﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace Roslynator.Formatting.CSharp
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class AddNewLineAfterOpeningBraceOfEmptyBlockAnalyzer : BaseDiagnosticAnalyzer
    {
        private static ImmutableArray<DiagnosticDescriptor> _supportedDiagnostics;

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                if (_supportedDiagnostics.IsDefault)
                    Immutable.InterlockedInitialize(ref _supportedDiagnostics, DiagnosticRules.AddNewLineAfterOpeningBraceOfEmptyBlock);

                return _supportedDiagnostics;
            }
        }

        public override void Initialize(AnalysisContext context)
        {
            base.Initialize(context);

            context.RegisterSyntaxNodeAction(f => AnalyzeBlock(f), SyntaxKind.Block);
        }

        private static void AnalyzeBlock(SyntaxNodeAnalysisContext context)
        {
            var block = (BlockSyntax)context.Node;

            if (block.Statements.Any())
                return;

            //TODO: AccessorDeclarationSyntax
            if (block.Parent is AccessorDeclarationSyntax)
                return;

            if (block.Parent is AnonymousFunctionExpressionSyntax)
                return;

            SyntaxToken openBrace = block.OpenBraceToken;

            if (!block.SyntaxTree.IsSingleLineSpan(TextSpan.FromBounds(openBrace.Span.End, openBrace.GetNextToken().SpanStart)))
                return;

            DiagnosticHelpers.ReportDiagnostic(
                context,
                DiagnosticRules.AddNewLineAfterOpeningBraceOfEmptyBlock,
                Location.Create(block.SyntaxTree, new TextSpan(openBrace.Span.End, 0)));
        }
    }
}
