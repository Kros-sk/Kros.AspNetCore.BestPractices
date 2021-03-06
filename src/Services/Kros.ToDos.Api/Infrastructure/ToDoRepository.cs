﻿using Kros.KORM;
using Kros.KORM.Metadata.Attribute;
using Kros.ToDos.Api.Domain;
using Kros.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kros.ToDos.Api.Infrastructure
{
    /// <summary>
    /// Repository for persistating <see cref="ToDo"/>.
    /// </summary>
    public class ToDoRepository : IToDoRepository
    {
        private readonly IDatabase _database;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="database">Database</param>
        public ToDoRepository(IDatabase database)
        {
            _database = Check.NotNull(database, nameof(database));
        }

        /// <inheritdoc />
        public async Task CreateToDoAsync(ToDo toDo)
        {
            var todos = _database.Query<ToDo>().AsDbSet();

            toDo.Created = DateTimeOffset.Now;
            toDo.LastChange = DateTimeOffset.Now;
            toDo.IsDone = false;

            todos.Add(toDo);

            await todos.CommitChangesAsync();
        }

        /// <inheritdoc />
        public async Task UpdateToDoAsync(ToDo toDo)
        {
            var todos = _database
                .Query<ToDo>()
                .Select(_editColumns.Value)
                .AsDbSet();

            toDo.LastChange = DateTimeOffset.Now;
            todos.Edit(toDo);

            await todos.CommitChangesAsync();
        }

        /// <inheritdoc />
        public async Task DeleteToDoAsync(long id)
        {
            var todos = _database.Query<ToDo>().AsDbSet();
            todos.Delete(new ToDo() { Id = id });

            await todos.CommitChangesAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<long>> DeleteCompletedToDosAsync(long userId, long organizationId)
        {
            using (var transaction = _database.BeginTransaction())
            {
                try
                {
                    var todoIds = _database
                        .Query<long>()
                        .Sql($"SELECT Id FROM ToDos " +
                             $"WHERE (UserId = {userId} AND OrganizationId = {organizationId}) AND (IsDone = 1)")
                        .ToList();
                    await _database.ExecuteNonQueryAsync($"DELETE FROM ToDos " +
                                                         $"WHERE (UserId = {userId} AND OrganizationId = {organizationId}) " +
                                                          "AND (IsDone = 1)");

                    transaction.Commit();

                    return todoIds;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <inheritdoc />
        public async Task ChangeIsDoneState(long id, bool isDone)
        {
            var todos = _database.Query<ToDoIsDoneEdit>().AsDbSet();
            todos.Edit(new ToDoIsDoneEdit() { Id = id, IsDone = isDone });

            await todos.CommitChangesAsync();
        }

        // Dočasne pokiaľ KORM nevie injektovať Created a LastChange
        private static readonly Lazy<string[]> _editColumns
            = new Lazy<string[]>(()
                => typeof(ToDo).GetProperties()
                .Where(p => p.Name != nameof(ToDo.Created))
                .Select(p => p.Name)
                .ToArray());


        [Alias("ToDos")]
        private class ToDoIsDoneEdit
        {
            [Key]
            public long Id { get; set; }

            public bool IsDone { get; set; }
        }
    }
}
