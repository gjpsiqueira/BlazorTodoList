using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using TodoList.Data;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;
using System.Text.Json.Serialization;
using Microsoft.JSInterop;

namespace TodoList.Pages
{
    public partial class Todo : IAsyncDisposable
    {

        private EditContext editContext;
        private TodoItem todoItem;
        private List<TodoItem> todoItemList = new List<TodoItem>();
        private IJSObjectReference jSObjectReference;

        [Inject] private ProtectedLocalStorage BrowserStorage { get; set; }
        [Inject] private IJSRuntime jSRuntime { get; set; }

        protected override void OnInitialized()
        {
            todoItem = new();
            editContext = new(todoItem);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                jSObjectReference = await jSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/todo.js");
                var localStorageList = await jSObjectReference.InvokeAsync<string>("getLocalStorage");

                if (localStorageList != null)
                {
                    List<TodoItem> a = JsonSerializer.Deserialize<List<TodoItem>>(localStorageList);
                    todoItemList.AddRange(a);
                    StateHasChanged();
                }
            }
                
        }

        public void HandleSubmit()
        {
            if(editContext != null && editContext.Validate())
            {
                Console.WriteLine("Teste " + todoItem.description);
                todoItemList.Add(new TodoItem()
                    {
                        description = todoItem.description,
                        dateCreated = DateTime.Now,
                        isComplete = false,
                        id = Guid.NewGuid()
                    }
                ); ;

                todoItem.description = "";
            }
            else
            {
                Console.WriteLine("Not Invalid!");
            }
            
        }

        public void HandleDelete(Guid guid)
        {
            todoItemList.Remove(todoItemList.Find(x => x.id == guid));
            StateHasChanged();
        }

        public void Dispose()
        {
            Console.WriteLine("Dispose");

        }

        public async ValueTask DisposeAsync()
        {

            await jSObjectReference.InvokeVoidAsync("setLocalStorage", todoItemList);
            
        }
    }
}
