import type { CategoryDto } from '../../Types';
import { httpService } from '../httpService';

const API_BASE = 'http://localhost:5000/api/budget/Category';

export const categoryService = {
    getCategory(): Promise<CategoryDto[]> {
        return httpService.get<{categories: CategoryDto[]}>(`${API_BASE}`)
        .then(response=>response.categories);
    },
};