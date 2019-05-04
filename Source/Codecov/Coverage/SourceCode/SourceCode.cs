﻿using System;
using System.Collections.Generic;
using System.Linq;
using Codecov.Services.VersionControlSystems;
using Codecov.Utilities;

namespace Codecov.Coverage.SourceCode
{
    internal class SourceCode : ISourceCode
    {
        private readonly Lazy<IEnumerable<string>> _getAll;
        private readonly Lazy<IEnumerable<string>> _getAllButCodecovIgnored;
        private readonly Lazy<string> _directory;
        private IPathFilter _fileFilter;

        public SourceCode(IVersionControlSystem versionControlSystem)
        {
            VersionControlSystem = versionControlSystem;
            _getAll = new Lazy<IEnumerable<string>>(() => VersionControlSystem.SourceCode.Select(FileSystem.NormalizedPath));
            _getAllButCodecovIgnored = new Lazy<IEnumerable<string>>(LoadGetAllButCodecovIgnored);
            _directory = new Lazy<string>(() => VersionControlSystem.RepoRoot);

            _fileFilter = BuildSourceFilter();
        }

        public IEnumerable<string> GetAll => _getAll.Value;

        public IEnumerable<string> GetAllButCodecovIgnored => _getAllButCodecovIgnored.Value;

        public string Directory => _directory.Value;

        private IVersionControlSystem VersionControlSystem { get; }

        private IEnumerable<string> LoadGetAllButCodecovIgnored()
        {
            return GetAll.Where(file => !_fileFilter.Matches(file));
        }

        private static IPathFilter BuildSourceFilter()
        {
            return new PathFilterBuilder()
                .PathContains(@"\.git")
                .PathContains(@"\bin\debug")
                .PathContains(@"\bin\release")
                .PathContains(@"\obj\debug")
                .PathContains(@"\obj\release")
                .PathContains(@"\.vscode")
                .PathContains(@"\.vs")
                .PathContains(@"\obj\project.assets.json")
                .PathContains(@"\virtualenv")
                .PathContains(@"\.virtualenv")
                .PathContains(@"\virtualenvs")
                .PathContains(@"\.virtualenvs")
                .PathContains(@"\env")
                .PathContains(@"\.env")
                .PathContains(@"\envs")
                .PathContains(@"\.envs")
                .PathContains(@"\venv")
                .PathContains(@"\.venv")
                .PathContains(@"\venvs")
                .PathContains(@"\.venvs")
                .PathContains(@"\build\lib")
                .PathContains(@"\.egg-info")
                .PathContains(@"\shunit2-2.1.6")
                .PathContains(@"\vendor")
                .PathContains(@"\js\generated\coverage")
                .PathContains(@"\__pycache__")
                .PathContains(@"\__pycache__")
                .PathContains(@"\node_modules")
                .PathContains(@".csproj.nuget.g.targets")
                .PathContains(".csproj.nuget.g.props")
                .HasExtension(".dll")
                .HasExtension(".exe")
                .HasExtension(".gif")
                .HasExtension(".jpg")
                .HasExtension(".jpeg")
                .HasExtension(".md")
                .HasExtension(".png")
                .HasExtension(".psd")
                .HasExtension(".ptt")
                .HasExtension(".pptx")
                .HasExtension(".numbers")
                .HasExtension(".pages")
                .HasExtension(".txt")
                .HasExtension(".xlsx")
                .HasExtension(".docx")
                .HasExtension(".doc")
                .HasExtension(".pdf")
                .HasExtension(".yml")
                .HasExtension(".yaml")
                .HasExtension(".gitignore")
                .Build();
        }
    }
}
