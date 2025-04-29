import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { Todo } from './types';
import { v4 as uuidv4 } from 'uuid';

const initialState: Todo[] = [];

const todoSlice = createSlice({
  name: 'todos',
  initialState,
  reducers: {
    addTodo: {
      reducer: (state, action: PayloadAction<Todo>) => {
        state.push(action.payload);
      },
      prepare: (text: string) => ({
        payload: {
          id: uuidv4(),
          text,
          completed: false,
        },
      }),
    },
    toggleTodo(state, action: PayloadAction<string>) {
      const todo = state.find(todo => todo.id === action.payload);
      if (todo) todo.completed = !todo.completed;
    },
    editTodo: (state, action: PayloadAction<{id: string; text: string}>) => {
      const todo = state.find(todo => todo.id === action.payload.id);
      if (todo) todo.text = action.payload.text;
    },
    deleteTodo: (state, action: PayloadAction<string>) => {
      return state.filter(todo => todo.id !== action.payload);
    },
  },
});

export const { addTodo, toggleTodo, editTodo, deleteTodo } = todoSlice.actions;
export default todoSlice.reducer;