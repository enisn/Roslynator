﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Pihrtsoft.CodeAnalysis.CSharp.Refactoring
{
    internal static class AnonymousMethodExpressionRefactoring
    {
        public static void ComputeRefactorings(RefactoringContext context, AnonymousMethodExpressionSyntax anonymousMethod)
        {
            if (context.Settings.IsRefactoringEnabled(RefactoringIdentifiers.ReplaceAnonymousMethodWithLambdaExpression)
                && anonymousMethod.DelegateKeyword.Span.Contains(context.Span)
                && ReplaceAnonymousMethodWithLambdaExpressionRefactoring.CanRefactor(anonymousMethod))
            {
                context.RegisterRefactoring(
                    "Replace anonymous method with lambda expression",
                    cancellationToken =>
                    {
                        return ReplaceAnonymousMethodWithLambdaExpressionRefactoring.RefactorAsync(
                            context.Document,
                            anonymousMethod,
                            cancellationToken);
                    });
            }
        }
    }
}
