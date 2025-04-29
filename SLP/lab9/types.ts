export interface Todo {
    id: string;
    text: string;
    completed: boolean;
  }
  
  export const ADD_TODO = 'ADD_TODO';
  export const TOGGLE_TODO = 'TOGGLE_TODO';
  export const EDIT_TODO = 'EDIT_TODO';
  export const DELETE_TODO = 'DELETE_TODO';
  
  export type TodoActionTypes =
    | { type: typeof ADD_TODO; payload: string }
    | { type: typeof TOGGLE_TODO; payload: string }
    | { type: typeof EDIT_TODO; payload: { id: string; text: string } }
    | { type: typeof DELETE_TODO; payload: string };