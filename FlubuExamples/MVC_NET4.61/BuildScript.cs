﻿using System;
using System.Collections.Generic;
using FlubuCore.Context;
using FlubuCore.Scripting;
using FlubuCore.Targeting;
using FlubuCore.Tasks.Testing;


public class BuildScript : DefaultBuildScript
{
    protected override void ConfigureBuildProperties(IBuildPropertiesContext context)
    {
        context.Properties.Set(BuildProps.NUnitConsolePath,
            @"..\\FlubuExamples\\MVC_NET4.61\\packages\NUnit.ConsoleRunner.3.2.1\tools\nunit3-console.exe");
        context.Properties.Set(BuildProps.ProductId, "FlubuExample");
        context.Properties.Set(BuildProps.ProductName, "FlubuExample");
        context.Properties.Set(BuildProps.SolutionFileName, "..\\FlubuExamples\\MVC_NET4.61\\FlubuExample.sln");
        context.Properties.Set(BuildProps.BuildConfiguration, "Release");

    }

    protected override void ConfigureTargets(ITaskContext session)
    {
        var loadSolution = session.CreateTarget("load.solution")
            .AddTask(x => x.LoadSolutionTask());

        var compile = session.CreateTarget("compile")
            .AddTask(x => x.CompileSolutionTask())
            .DependsOn(loadSolution);

        var unitTests = session.CreateTarget("unit.tests")
            .AddTask(x => x.NUnitTaskForNunitV3("FlubuExample.Tests"));

        session.CreateTarget("Rebuild")
            .SetAsDefault()
            .DependsOn(compile, unitTests);
    }
}
