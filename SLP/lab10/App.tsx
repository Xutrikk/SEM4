import { useState } from 'react';
import { useSelector } from 'react-redux';
import { RootState } from './app/store';
import AddTodoForm from './components/AddTodoForm';
import TodoList from './components/TodoList';

const App = () => {
  const todos = useSelector((state: RootState) => state.todos);
  const [editingTodo, setEditingTodo] = useState<{id: string; text: string} | null>(null);

  return (
    <div className="app-container">
      <h1>Todo List (RTK)</h1>
      <AddTodoForm 
        editingTodo={editingTodo}
        clearEditing={() => setEditingTodo(null)}
      />
      <TodoList 
        todos={todos} 
        onEditStart={(id, text) => setEditingTodo({id, text})}
      />
    </div>
  );
};

export default App;