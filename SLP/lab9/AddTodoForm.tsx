import { useState, useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { addTodo, editTodo } from '../redux/actions/todoActions';

interface AddTodoFormProps {
  editingTodo: {id: string; text: string} | null;
  clearEditing: () => void;
}

const AddTodoForm = ({ editingTodo, clearEditing }: AddTodoFormProps) => {
  const [text, setText] = useState('');
  const dispatch = useDispatch();

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
      dispatch(editTodo(editingTodo.id, text));
    } else {
      dispatch(addTodo(text));
    }
    clearEditing();
    setText('');
  };

  const handleCancel = () => {
    clearEditing();
    setText(''); 
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
        {editingTodo && (
          <button 
            type="button" 
            onClick={handleCancel}
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