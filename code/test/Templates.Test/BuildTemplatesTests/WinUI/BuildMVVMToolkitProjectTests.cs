﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Extensions;
using Xunit;

namespace Microsoft.Templates.Test.Build.WinUI
{
    [Collection("BuildTemplateTestCollection")]
    public class BuildMVVMToolkitProjectTests : BaseGenAndBuildTests
    {
        public BuildMVVMToolkitProjectTests(BuildTemplatesTestFixture fixture)
            : base(fixture, null, Frameworks.MVVMToolkit)
        {
        }

        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetProjectTemplatesForBuild), Frameworks.MVVMToolkit, ProgrammingLanguages.CSharp, Platforms.WinUI)]
        [Trait("ExecutionSet", "BuildMVVMToolkitWinUI")]
        [Trait("ExecutionSet", "_Full")]
        [Trait("Type", "BuildProjects")]
        public async Task Build_EmptyProject_MVVMToolkitAsync(string projectType, string framework, string platform, string language)
        {
            var (projectName, projectPath) = await GenerateEmptyProjectAsync(projectType, framework, platform, language);

            // Don't delete after build test as used in inference test, which will then delete.
            AssertBuildProject(projectPath, projectName, platform);

        }

        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetProjectTemplatesForBuild), Frameworks.MVVMToolkit, ProgrammingLanguages.CSharp, Platforms.WinUI)]
        [Trait("ExecutionSet", "BuildMVVMToolkitWinUI")]
        [Trait("ExecutionSet", "_Full")]
        [Trait("Type", "BuildAllPagesAndFeatures")]
        [Trait("Type", "BuildRandomNames")]
        public async Task Build_All_ProjectNameValidation_WinUIAsync(string projectType, string framework, string platform, string language)
        {
            bool templateSelector(ITemplateInfo t) => t.GetTemplateType().IsItemTemplate()
                && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                && (t.GetFrontEndFrameworkList().Contains(framework) || t.GetFrontEndFrameworkList().Contains(All))
                && t.GetPlatform() == platform
                && !t.GetIsHidden();

            var projectName = $"{ShortProjectType(projectType)}{CharactersThatMayCauseProjectNameIssues()}";

            var projectPath = await AssertGenerateProjectAsync(projectName, projectType, framework, platform, language, templateSelector, BaseGenAndBuildFixture.GetRandomName);

            AssertBuildProject(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetProjectTemplatesForBuild), Frameworks.MVVMToolkit, ProgrammingLanguages.CSharp, Platforms.WinUI)]
        [Trait("ExecutionSet", "MinimumWinUI")]
        [Trait("ExecutionSet", "MinimumMVVMToolkitWinUI")]
        [Trait("ExecutionSet", "_CIBuild")]
        [Trait("ExecutionSet", "_Full")]
        [Trait("Type", "CodeStyle")]
        public async Task BuildAndTest_All_CheckWithStyleCop_WinUIAsync(string projectType, string framework, string platform, string language)
        {
            bool templateSelector(ITemplateInfo t) => t.GetTemplateType().IsItemTemplate()
                && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                && (t.GetFrontEndFrameworkList().Contains(framework) || t.GetFrontEndFrameworkList().Contains(All))
                && t.GetPlatform() == platform
                && !t.GetIsHidden()
                || t.Identity == "wts.WinUI.Feat.StyleCop";

            var projectName = $"{projectType}{framework}AllStyleCop";

            var projectPath = await AssertGenerateProjectAsync(projectName, projectType, framework, platform, language, templateSelector, BaseGenAndBuildFixture.GetDefaultName, true);

            AssertBuildProject(projectPath, projectName, platform);
        }


        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetProjectTemplatesForBuild), Frameworks.MVVMToolkit, "", Platforms.WinUI)]
        [Trait("ExecutionSet", "BuildMVVMToolkitWinUI")]
        [Trait("ExecutionSet", "_Full")]
        [Trait("Type", "BuildRightClick")]
        public async Task Build_Empty_AddRightClick_WinUIAsync(string projectType, string framework, string platform, string language)
        {
            var projectName = $"{ShortProjectType(projectType)}AllRC";

            var projectPath = await AssertGenerateRightClickAsync(projectName, projectType, framework, platform, language, true);

            AssertBuildProject(projectPath, projectName, platform);
        }

        [Theory]
        [MemberData(nameof(BaseGenAndBuildTests.GetPageAndFeatureTemplatesForBuild), Frameworks.MVVMToolkit, ProgrammingLanguages.CSharp, Platforms.WinUI, "")]
        [Trait("ExecutionSet", "BuildOneByOneMVVMToolkitWinUI")]
        [Trait("ExecutionSet", "_OneByOne")]
        [Trait("Type", "BuildOneByOneMVVMToolkitWinUI")]
        public async Task Build_MVVMToolkit_OneByOneItems_WinUIAsync(string itemName, string projectType, string framework, string platform, string itemId, string language)
        {
            var (ProjectPath, ProjecName) = await AssertGenerationOneByOneAsync(itemName, projectType, framework, platform, itemId, language, false);

            AssertBuildProject(ProjectPath, ProjecName, platform);
        }
    }
}
