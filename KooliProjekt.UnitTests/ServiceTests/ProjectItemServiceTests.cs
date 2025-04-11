using Kooliprojekt.Data;
using Kooliprojekt.Search;
using Kooliprojekt.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class ProjectItemServiceTests : ServiceTestBase
    {
        private readonly ProjectItemService _service;

        public ProjectItemServiceTests()
        {
            _service = new ProjectItemService(DbContext);  // Use the in-memory DbContext from ServiceTestBase
        }

        [Fact]
        public async Task List_should_return_paginated_result_without_search()
        {
            // Arrange
            var projectItems = new List<ProjectIList>
            {
                new ProjectIList { Id = 1, Title = "Item 1" },
                new ProjectIList { Id = 2, Title = "Item 2" }
            };

            DbContext.ProjectItem.AddRange(projectItems);
            await DbContext.SaveChangesAsync();

            var page = 1;
            var pageSize = 10;

            // Act
            var result = await _service.List(page, pageSize, null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Results.Count); // Should return both items
        }

        [Fact]
        public async Task List_should_apply_search_filter_when_keyword_is_provided()
        {
            // Arrange
            var projectItems = new List<ProjectIList>
            {
                new ProjectIList { Id = 1, Title = "Item 1" },
                new ProjectIList { Id = 2, Title = "Item 2" },
                new ProjectIList { Id = 3, Title = "Test Item" }
            };

            DbContext.ProjectItem.AddRange(projectItems);
            await DbContext.SaveChangesAsync();

            var search = new ProjectItemSearch { Keyword = "Test" };
            var page = 1;
            var pageSize = 10;

            // Act
            var result = await _service.List(page, pageSize, search);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Results); // Should only return "Test Item"
        }

        [Fact]
        public async Task List_should_apply_done_filter_when_isdone_is_provided()
        {
            // Arrange
            var projectItems = new List<ProjectIList>
            {
                new ProjectIList { Id = 1, Title = "Item 1", IsDone = true },
                new ProjectIList { Id = 2, Title = "Item 2", IsDone = false }
            };

            DbContext.ProjectItem.AddRange(projectItems);
            await DbContext.SaveChangesAsync();

            var search = new ProjectItemSearch { IsDone = true };
            var page = 1;
            var pageSize = 10;

            // Act
            var result = await _service.List(page, pageSize, search);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Results); // Should only return the item that is done
            Assert.True(result.Results[0].IsDone);
        }

        [Fact]
        public async Task Get_should_return_item_when_found()
        {
            // Arrange
            var expectedItem = new ProjectIList { Id = 1, Title = "Test Item" };
            DbContext.ProjectItem.Add(expectedItem);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _service.Get(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task Get_should_return_null_when_not_found()
        {
            // Act
            var result = await _service.Get(999);  // Item with Id = 999 does not exist

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Save_should_add_new_item_when_id_is_zero()
        {
            // Arrange
            var newItem = new ProjectIList { Id = 0, Title = "New Item" };

            // Act
            await _service.Save(newItem);
            await DbContext.SaveChangesAsync();

            // Assert
            var savedItem = await DbContext.ProjectItem.FindAsync(newItem.Id);
            Assert.NotNull(savedItem);
            Assert.Equal("New Item", savedItem.Title);
        }

        [Fact]
        public async Task Save_should_update_existing_item_when_id_is_non_zero()
        {
            // Arrange
            var existingItem = new ProjectIList { Id = 1, Title = "Existing Item" };
            DbContext.ProjectItem.Add(existingItem);
            await DbContext.SaveChangesAsync();

            // Act
            existingItem.Title = "Updated Item";
            await _service.Save(existingItem);
            await DbContext.SaveChangesAsync();

            // Assert
            var updatedItem = await DbContext.ProjectItem.FindAsync(existingItem.Id);
            Assert.NotNull(updatedItem);
            Assert.Equal("Updated Item", updatedItem.Title);
        }

        [Fact]
        public async Task Delete_should_remove_item_when_found()
        {
            // Arrange
            var item = new ProjectIList { Id = 1, Title = "Item to Delete" };
            DbContext.ProjectItem.Add(item);
            await DbContext.SaveChangesAsync();

            // Act
            await _service.Delete(1);
            await DbContext.SaveChangesAsync();

            // Assert
            var deletedItem = await DbContext.ProjectItem.FindAsync(1);
            Assert.Null(deletedItem);
        }
    }
}
