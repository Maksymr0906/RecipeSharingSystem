import { Component, OnDestroy, OnInit } from '@angular/core';
import { AddCategoryRequest } from '../../models/add-category-request.model';
import { Subscription } from 'rxjs';
import { ImageService } from 'src/app/shared/services/image.service';
import { CategoryService } from '../services/category.service';

@Component({
  selector: 'app-add-category',
  templateUrl: './add-category.component.html',
  styleUrls: ['./add-category.component.css']
})
export class AddCategoryComponent implements OnInit, OnDestroy {
  model: AddCategoryRequest;
  isImageSelectorVisible: boolean = false;
  imageSelectorSubscription?: Subscription;
  addCategorySubscription?: Subscription;

  constructor(
    private imageService: ImageService,
    private categoryService: CategoryService
  )
  {
    this.model = {
      name: '',
      imageId: ''
    };
  }

  onFormSubmit() {
    this.addCategorySubscription = this.categoryService.addCategory(this.model).subscribe(
      category => {
        console.log(category);
      }
    );
  }

  openImageSelector(): void {
    this.isImageSelectorVisible = true;
  }

  closeImageSelector(): void {
    this.isImageSelectorVisible = false;
  }

  ngOnInit(): void {
    this.imageSelectorSubscription = this.imageService.onSelectImage().subscribe({
      next: (response) => {
        this.model.imageId = response.id;
        this.closeImageSelector();
      }
    })
  }

  ngOnDestroy(): void {
    this.imageSelectorSubscription?.unsubscribe();
    this.addCategorySubscription?.unsubscribe();
  }
}
