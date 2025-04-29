import { Todo } from '../redux/types';
import { useDispatch } from 'react-redux';
import { toggleTodo, deleteTodo } from '../redux/actions/todoActions';

interface TodoItemProps {
  todo: Todo;
  onEditStart: (id: string, text: string) => void;
}

const TodoItem = ({ todo, onEditStart }: TodoItemProps) => {
  const dispatch = useDispatch();

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