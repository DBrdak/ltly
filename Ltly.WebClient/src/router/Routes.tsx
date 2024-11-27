import { RouteObject } from "react-router"
import {createBrowserRouter} from 'react-router-dom'
import App from "../App";
import React from "react";

export const routes: RouteObject[] = [
    {
        path: '/',
        element: <App />,
        children: [
            {path: '*', element: <App />},
        ]
    }
]

export const router = createBrowserRouter(routes);