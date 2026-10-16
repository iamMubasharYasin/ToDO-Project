import { Component, computed, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

type Todo = {
  id: number;
  title: string;
  category: string;
  done: boolean;
};

type TodoFilter = 'all' | 'active' | 'completed';

@Component({
  imports: [RouterOutlet],
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html',
})
export class App {
  protected readonly todos = signal<Todo[]>([
    { id: 1, title: 'Review project proposal', category: 'Work', done: false },
    { id: 2, title: 'Prepare slides for the team', category: 'Work', done: false },
    { id: 3, title: 'Pick up groceries', category: 'Personal', done: false },
    { id: 4, title: 'Read 20 pages', category: 'Personal', done: true },
  ]);
  protected readonly filter = signal<TodoFilter>('all');
  protected readonly visibleTodos = computed(() => {
    const selected = this.filter();
    return this.todos().filter((todo) => {
      if (selected === 'active') return !todo.done;
      if (selected === 'completed') return todo.done;
      return true;
    });
  });
  protected readonly remainingCount = computed(() => this.todos().filter((todo) => !todo.done).length);
  protected readonly completedCount = computed(() => this.todos().filter((todo) => todo.done).length);

  protected addTodo(title: string, input: HTMLInputElement): void {
    const cleanTitle = title.trim();
    if (!cleanTitle) return;

    this.todos.update((todos) => [
      { id: Date.now(), title: cleanTitle, category: 'Personal', done: false },
      ...todos,
    ]);
    input.value = '';
    input.focus();
  }

  protected toggleTodo(id: number): void {
    this.todos.update((todos) => todos.map((todo) => todo.id === id ? { ...todo, done: !todo.done } : todo));
  }

  protected deleteTodo(id: number): void {
    this.todos.update((todos) => todos.filter((todo) => todo.id !== id));
  }

  protected clearCompleted(): void {
    this.todos.update((todos) => todos.filter((todo) => !todo.done));
  }

  protected setFilter(filter: TodoFilter): void {
    this.filter.set(filter);
  }
}
