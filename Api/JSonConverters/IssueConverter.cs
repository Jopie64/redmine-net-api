/*
   Copyright 2011 - 2013 Adrian Popescu, Dorin Huzum.

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

//#if RUNNING_ON_35_OR_ABOVE
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Script.Serialization;
using Redmine.Net.Api.Types;

namespace Redmine.Net.Api.JSonConverters
{
    public class IssueConverter : JavaScriptConverter
    {
        #region Overrides of JavaScriptConverter

        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            if (dictionary != null)
            {
                var issue = new Issue();

                issue.Id = dictionary.GetValue<int>("id");
                issue.Description = dictionary.GetValue<string>("description");
                issue.Project = dictionary.GetValueAsIdentifiableName("project");
                issue.Tracker = dictionary.GetValueAsIdentifiableName("tracker");
                issue.Status = dictionary.GetValueAsIdentifiableName("status");
                issue.CreatedOn = dictionary.GetValue<DateTime?>("created_on");
                issue.UpdatedOn = dictionary.GetValue<DateTime?>("updated_on");
                issue.Priority = dictionary.GetValueAsIdentifiableName("priority");
                issue.Author = dictionary.GetValueAsIdentifiableName("author");
                issue.AssignedTo = dictionary.GetValueAsIdentifiableName("assigned_to");
                issue.Category = dictionary.GetValueAsIdentifiableName("category");
                issue.FixedVersion = dictionary.GetValueAsIdentifiableName("fixed_version");
                issue.Subject = dictionary.GetValue<string>("subject");
                issue.Notes = dictionary.GetValue<string>("notes");
                issue.StartDate = dictionary.GetValue<DateTime?>("start_date");
                issue.DueDate = dictionary.GetValue<DateTime?>("due_date");
                issue.DoneRatio = dictionary.GetValue<float>("done_ratio");
                issue.EstimatedHours = dictionary.GetValue<float>("estimated_hours");
                issue.ParentIssue = dictionary.GetValueAsIdentifiableName("parent");

                issue.CustomFields = dictionary.GetValueAsCollection<CustomField>("custom_fields");
                issue.Attachments = dictionary.GetValueAsCollection<Attachment>("attachments");
                issue.Relations = dictionary.GetValueAsCollection<IssueRelation>("relations");
                issue.Journals = dictionary.GetValueAsCollection<Journal>("journals");
                issue.Changesets = dictionary.GetValueAsCollection<ChangeSet>("changesets");
                issue.Watchers = dictionary.GetValueAsCollection<Watcher>("watchers");
                return issue;
            }

            return null;
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            var entity = obj as Issue;
            var root = new Dictionary<string, object>();
            var result = new Dictionary<string, object>();

            if (entity != null)
            {
                result.Add("subject", entity.Subject);
                result.Add("description", entity.Description);
                result.Add("notes", entity.Notes);
                result.WriteIdIfNotNull(entity.Project, "project_id");
                result.WriteIdIfNotNull(entity.Priority, "priority_id");
                result.WriteIdIfNotNull(entity.Status, "status_id");
                result.WriteIdIfNotNull(entity.Category, "category_id");
                result.WriteIdIfNotNull(entity.Tracker, "tracker_id");
                result.WriteIdIfNotNull(entity.AssignedTo, "assigned_to_id");
                result.WriteIdIfNotNull(entity.FixedVersion, "fixed_version");
                result.WriteIdIfNotNull(entity.ParentIssue, "parent_issue_id");
                result.WriteIfNotDefaultOrNull(entity.EstimatedHours, "estimated_hours");

                if (entity.StartDate != null)
                    result.Add("start_date", entity.StartDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
                if (entity.DueDate != null)
                    result.Add("due_date", entity.DueDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

                result.WriteIfNotDefaultOrNull(entity.DoneRatio, "done_ratio");

                if (entity.Uploads != null) result.Add("uploads", entity.Uploads.ToArray());

                if (entity.CustomFields != null)
                {
                    serializer.RegisterConverters(new[] { new CustomFieldConverter() });
                    result.Add("custom_fields", entity.CustomFields.ToArray());
                }

                if (entity.Watchers != null)
                {
                    serializer.RegisterConverters(new[] { new WatcherConverter()  });
                    result.Add("watchers", entity.Watchers.ToArray());
                }

                root["issue"] = result;
                return root;
            }

            return result;
        }

        public override IEnumerable<Type> SupportedTypes { get { return new List<Type>(new[] { typeof(Issue) }); } }

        #endregion
    }
}
//#endif