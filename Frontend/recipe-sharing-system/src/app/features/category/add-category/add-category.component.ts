import { Component, OnDestroy, OnInit } from '@angular/core';
import { AddCategoryRequest } from '../models/add-category-request.model';
import { Subscription } from 'rxjs';
import { ImageService } from 'src/app/shared/services/image.service';
import { CategoryService } from '../services/category.service';
import { Router } from '@angular/router';

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
    private categoryService: CategoryService,
    private router: Router
  )
  {
    this.model = {
      name: '',
      slug: '',
      featuredImageUrl: ''
    };
  }

  onFormSubmit() {
    this.addCategorySubscription = this.categoryService.addCategory(this.model).subscribe(
      _ => {
        this.router.navigateByUrl('/admin/categories');
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
        this.model.featuredImageUrl = response.url;
        this.closeImageSelector();
      }
    })
  }

  ngOnDestroy(): void {
    this.imageSelectorSubscription?.unsubscribe();
    this.addCategorySubscription?.unsubscribe();
    this.imageService.resetSelectedImage();
  }
}
