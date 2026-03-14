import type { CategoryDto } from '../../Types';
import { httpService } from '../httpService';

const API_BASE = '/api/budget/Category';

export const categoryService = {
    getCategory(): Promise<CategoryDto[]> {
        return httpService.get<{categories: CategoryDto[]}>(`${API_BASE}`)
        .then(response=>response.categories);
    },
};