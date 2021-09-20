﻿using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kros.Tags.Api.Application.Services
{
    /// <summary>
    /// Service providing methods for managing table storage
    /// </summary>
    public class TableStorageManagementService : ITableStorageManagementService
    {
        private const string ConnectionString = "UseDevelopmentStorage=true";
        private const string TableName = "colors";

        private CloudStorageAccount _account;
        private CloudTableClient _tableClient;
        private Lazy<CloudTable> _table;

        /// <summary>
        /// Ctor.
        /// </summary>
        public TableStorageManagementService()
        {
            _account = CloudStorageAccount.Parse(ConnectionString);
            _tableClient = _account.CreateCloudTableClient();
            _table = new Lazy<CloudTable>(InitializeTable(TableName));
        }

        /// <inheritdoc />
        public async Task AddRow(long partitionKey, int rowKey)
        {
            var color = new ColorEntity(partitionKey, rowKey);

            TableOperation operation = TableOperation.Insert(color);

            await _table.Value.ExecuteAsync(operation);
        }

        /// <inheritdoc />
        public async Task DeleteValue(long partitionKey, int rowKey)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<ColorEntity>(partitionKey.ToString(), rowKey.ToString());
            var result = await _table.Value.ExecuteAsync(retrieveOperation);
            var color = result.Result as ColorEntity;

            TableOperation operation = TableOperation.Delete(color);

            await _table.Value.ExecuteAsync(operation);
        }

        /// <inheritdoc />
        public IEnumerable<ColorEntity> GetAllValuesForPartition(long partitionKey)
        {
            var query = new TableQuery<ColorEntity>();
            query.FilterString = TableQuery.GenerateFilterCondition(
                nameof(ColorEntity.OrganizationId),
                QueryComparisons.Equal,
                partitionKey.ToString());

            var results = _table.Value.ExecuteQuery(query);

            return results;
        }

        /// <inheritdoc />
        private CloudTable InitializeTable(string tableName)
        {
            var table = _tableClient.GetTableReference(tableName);

            table.CreateIfNotExists();
            return table;
        }

        ///<inheritdoc/>
        public async Task DeleteAllValuesForPartitionKey(long partitionKey)
        {
            var query = new TableQuery<ColorEntity>();
            query.FilterString = TableQuery.GenerateFilterCondition(
                nameof(ColorEntity.OrganizationId),
                QueryComparisons.Equal,
                partitionKey.ToString());
            var results = _table.Value.ExecuteQuery(query);
            foreach (var color in results)
            {
                TableOperation operation = TableOperation.Delete(color);
                await _table.Value.ExecuteAsync(operation);
            }
        }
    }
}
