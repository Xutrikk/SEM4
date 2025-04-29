import { Todo } from '../features/todo/types';
import TodoItem from './TodoItem';

interface TodoListProps {
  todos: Todo[];
  onEditStart: (id: string, text: string) => void;
}

const TodoList = ({ todos, onEditStart }: TodoListProps) => {
  return (
    <div className="todo-list">
      {todos.map((todo) => (
        <TodoItem 
          key={todo.id} 
          todo={todo} 
          onEditStart={onEditStart} 
        />
      ))}
    </div>
  );
};

export default TodoList;