﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator
{
    public static class ProjectExtensions
    {
        public static ImmutableArray<Diagnostic> GetAnalyzerDiagnostics(
            this Project project,
            DiagnosticAnalyzer analyzer,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetAnalyzerDiagnosticsAsync(project, analyzer, cancellationToken).Result;
        }

        public static async Task<ImmutableArray<Diagnostic>> GetAnalyzerDiagnosticsAsync(
            this Project project,
            DiagnosticAnalyzer analyzer,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Compilation compilation = await project.GetCompilationAsync(cancellationToken).ConfigureAwait(false);

            //TODO: del
            compilation = compilation.EnableDiagnosticsDisabledByDefault(analyzer);

            CompilationWithAnalyzers compilationWithAnalyzers = compilation.WithAnalyzers(ImmutableArray.Create(analyzer));

            return await compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}