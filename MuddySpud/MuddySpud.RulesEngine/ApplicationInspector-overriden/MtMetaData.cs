// Copyright (C) Microsoft. All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.ApplicationInspector.Commands;
using MuddySpud.Metrics.Engine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace MuddySpud.RulesEngine.Commands
{
    /// <summary>
    /// Contains the analyze scanned meta data elements including rollup data for reporting purposes
    /// </summary>
    public class MtMetaData
    {
        //simple properties
        /// <summary>
        /// Detected or derived project name
        /// </summary>
        [JsonProperty(PropertyName = "applicationName")]
        public string? ApplicationName { get; set; }

        /// <summary>
        /// Source path provided argument
        /// </summary>
        [JsonProperty(PropertyName = "sourcePath")]
        public string? SourcePath { get; set; }

        /// <summary>
        /// Detected project source version
        /// </summary>
        [JsonProperty(PropertyName = "sourceVersion")]
        public string? SourceVersion { get; set; }

        /// <summary>
        /// Detected source authors
        /// </summary>
        [JsonProperty(PropertyName = "authors")]
        public string? Authors { get; set; }

        /// <summary>
        /// Detected source description
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string? Description { get; set; }

        private readonly DateTime _lastUpdated = DateTime.MinValue;

        /// <summary>
        /// Last modified date for source code scanned
        /// </summary>
        [JsonProperty(PropertyName = "lastUpdated")]
        public string LastUpdated
        {
            //get
            //{
            //    if (Files.Any())
            //    {
            //        return Files.Select(x => x.ModifyTime).Max().ToString();
            //    }
            //    else
            //    {
            //        return DateTime.MinValue.ToString();
            //    }
            //}
            get
            {
                if (AbacusRecords.Any())
                {
                    return AbacusRecords.Select(x => x.File.ModifyTime).Max().ToString();
                }
                else
                {
                    return DateTime.MinValue.ToString();
                }
            }
        }

        /// <summary>
        /// Date of analyze scan
        /// </summary>
        [JsonProperty(PropertyName = "dateScanned")]
        public string? DateScanned { get; set; }

        //stats

        /// <summary>
        /// Total number of files in source path
        /// </summary>
        [JsonProperty(PropertyName = "totalFiles")]
        public int TotalFiles
        {
            get { return AbacusRecords.Count; }
        }

        /// <summary>
        /// Total number of files Timed out 
        /// </summary>
        [JsonProperty(PropertyName = "filesTimedOut")]
        public int FilesTimedOut
        {
            get { return AbacusRecords.Count(x => x.File.Status == ScanState.TimedOut); }
        }

        /// <summary>
        /// Total number of files scanned
        /// </summary>
        [JsonProperty(PropertyName = "filesAnalyzed")]
        public int FilesAnalyzed
        {
            get { return AbacusRecords.Count(x => x.File.Status == ScanState.Analyzed || x.File.Status == ScanState.Affected); }
        }

        /// <summary>
        /// Total number of skipped files based on supported formats
        /// </summary>
        [JsonProperty(PropertyName = "filesSkipped")]
        public int FilesSkipped
        {
            get { return AbacusRecords.Count(x => x.File.Status == ScanState.Skipped); }
        }

        /// <summary>
        /// Total files with at least one result
        /// </summary>
        [JsonProperty(PropertyName = "filesAffected")]
        public int FilesAffected
        {
            get { return AbacusRecords.Count(x => x.File.Status == ScanState.Affected); }
        }

        /// <summary>
        /// Total files with at least one result
        /// </summary>
        [JsonProperty(PropertyName = "filesErrored")]
        public int FileErrored
        {
            get { return AbacusRecords.Count(x => x.File.Status == ScanState.Error); }
        }

        /// <summary>
        /// Total matches with supplied argument settings
        /// </summary>
        [JsonProperty(PropertyName = "totalMatchesCount")]
        public int TotalMatchesCount
        {
            get { return AbacusRecords?.SelectMany(a => a.Metrics.Matches).Count() ?? 0; }
        }

        /// <summary>
        /// Total unique matches by Rule Id
        /// </summary>
        [JsonProperty(PropertyName = "uniqueMatchesCount")]
        public int UniqueMatchesCount
        {
            //get { return Metrics?.Select(x => x.RuleId).Distinct().Count() ?? 0; }
            get { return AbacusRecords?.SelectMany(a => a.Metrics.Matches).Select(x => x.RuleId).Distinct().Count() ?? 0; }
        }

        /// <summary>
        /// List of detected package types 
        /// </summary>
        [JsonProperty(PropertyName = "packageTypes")]
        public List<string>? PackageTypes { get; set; } = new();

        /// <summary>
        /// List of detected application types
        /// </summary>
        [JsonProperty(PropertyName = "appTypes")]
        public List<string>? AppTypes { get; set; } = new();

        /// <summary>
        /// List of detected unique tags 
        /// </summary>
        [JsonProperty(PropertyName = "uniqueTags")]
        public List<string>? UniqueTags { get; set; } = new();

        /// <summary>
        /// List of detected unique code dependency includes
        /// </summary>
        [JsonProperty(PropertyName = "uniqueDependencies")]
        public List<string>? UniqueDependencies { get; set; } = new();

        /// <summary>
        /// List of detected output types
        /// </summary>
        [JsonProperty(PropertyName = "outputs")]
        public List<string>? Outputs { get; set; } = new();

        /// <summary>
        /// List of detected target types
        /// </summary>
        [JsonProperty(PropertyName = "targets")]
        public List<string> Targets { get; set; } = new();

        /// <summary>
        /// List of detected OS targets
        /// </summary>
        [JsonProperty(PropertyName = "OSTargets")]
        public List<string>? OSTargets { get; set; } = new();

        /// <summary>
        /// LIst of detected file types (extension based)
        /// </summary>
        [JsonProperty(PropertyName = "fileExtensions")]
        public List<string>? FileExtensions { get; set; } = new();

        /// <summary>
        /// List of detected cloud host targets
        /// </summary>
        [JsonProperty(PropertyName = "cloudTargets")]
        public List<string>? CloudTargets { get; set; } = new();

        /// <summary>
        /// List of detected cpu targets
        /// </summary>
        [JsonProperty(PropertyName = "CPUTargets")]
        public List<string>? CPUTargets { get; set; } = new();

        /// <summary>
        /// List of detected programming languages used and count of files 
        /// </summary>
        [JsonProperty(PropertyName = "languages")]
        public ImmutableSortedDictionary<string, int>? Languages { get; set; } //unable to init here for constr arg

        /// <summary>
        /// List of detected tag counters i.e. metrics
        /// </summary>
        [JsonProperty(PropertyName = "tagCounters")]
        public List<MetricTagCounter>? TagCounters { get; set; } = new();

        ///// <summary>
        ///// List of detailed MatchRecords from scan
        ///// </summary>
        //[JsonProperty(PropertyName = "detailedMatchList2")]
        //public List<MtMatchRecord> Matches { get; set; } = new();

        /// <summary>
        /// List of detailed MatchRecords from scan
        /// </summary>
        [JsonProperty(PropertyName = "abacus")]
        public List<AbacusRecord> AbacusRecords { get; set; } = new();

        public MtMetaData(string applicationName, string sourcePath)
        {
            ApplicationName = applicationName;
            SourcePath = sourcePath;
        }
    }
}