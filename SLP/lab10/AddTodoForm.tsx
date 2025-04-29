import { useState, useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { AppDispatch } from '../app/store';
import { addTodo, editTodo } from '../features/todo/todoSlice';

interface AddTodoFormProps {
  editingTodo: { id: string; text: string } | null;
  clearEditing: () => void;
}

const AddTodoForm = ({ editingTodo, clearEditing }: AddTodoFormProps) => {
  const [text, setText] = useState('');
  const dispatch = useDispatch<AppDispatch>();

  useEffect(() => {
    if (editingTodo) {
      setText(editingTodo.text);
    } else {
      setText('');
    }
  }, [editingTodo]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!text.trim()) return;


    if (editingTodo) {
      dispatch(editTodo({ id: editingTodo.id, text }));
    } else {
      dispatch(addTodo(text));
    }
    
    clearEditing();
  };

  return (
    <form className="todo-form" onSubmit={handleSubmit}>
      <input
        type="text"
        value={text}
        onChange={(e) => setText(e.target.value)}
        placeholder="Введите задачу..."
      />
      <div className="form-buttons">
        <button type="submit">
          {editingTodo ? 'Сохранить' : 'Добавить'}
        </button>
        {}
        {editingTodo && (
          <button
            type="button"
            onClick={() => {
              clearEditing();
              setText('');
            }}
            className="cancel-button"
          >
            Отмена
          </button>
        )}
      </div>
    </form>
  );
};

export default AddTodoForm;