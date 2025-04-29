import { Todo } from '../features/todo/types';
import { useDispatch } from 'react-redux';
import { AppDispatch } from '../app/store';
import { toggleTodo, deleteTodo } from '../features/todo/todoSlice';

interface TodoItemProps {
  todo: Todo;
  onEditStart: (id: string, text: string) => void;
}

const TodoItem = ({ todo, onEditStart }: TodoItemProps) => {
  const dispatch = useDispatch<AppDispatch>();

  return (
    <div className="todo-item">
      <div className="todo-content">
        <input
          type="checkbox"
          checked={todo.completed}
          onChange={() => dispatch(toggleTodo(todo.id))}
        />
        <span className={todo.completed ? 'completed' : ''}>{todo.text}</span>
      </div>
      <div className="todo-actions">
        <button onClick={() => onEditStart(todo.id, todo.text)}>Редактировать</button>
        <button onClick={() => dispatch(deleteTodo(todo.id))}>Удалить</button>
      </div>
    </div>
  );
};

export default TodoItem;