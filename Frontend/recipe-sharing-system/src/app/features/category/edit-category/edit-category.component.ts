import { Component, OnDestroy, OnInit } from '@angular/core';
import { EditCategoryRequest } from '../models/edit-category-request.model';
import { Subscription } from 'rxjs';
import { ImageService } from 'src/app/shared/services/image.service';
import { CategoryService } from '../services/category.service';
import { ActivatedRoute, Route, Router } from '@angular/router';

@Component({
  selector: 'app-edit-category',
  templateUrl: './edit-category.component.html',
  styleUrls: ['./edit-category.component.css']
})
export class EditCategoryComponent implements OnInit, OnDestroy {
  id: string | null = null;
  model: EditCategoryRequest;
  isImageSelectorVisible: boolean = false;
  imageSelectorSubscription?: Subscription;
  updateCategorySubscription?: Subscription;
  getCategoryByIdSubscription?: Subscription;

  constructor(
    private imageService: ImageService,
    private categoryService: CategoryService,
    private router: Router,
    private route: ActivatedRoute
  )
  {
    this.model = {
      name: '',
      slug: '',
      featuredImageUrl: ''
    };
  }

  onFormSubmit() {
    if (this.id) {
      this.updateCategorySubscription = this.categoryService.updateCategory(this.id, this.model).subscribe(_ => {
        this.router.navigateByUrl('admin/categories');
      });
    }
  }

  openImageSelector(): void {
    this.isImageSelectorVisible = true;
  }

  closeImageSelector(): void {
    this.isImageSelectorVisible = false;
  }

  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id');
    if (this.id) {
      this.getCategoryByIdSubscription = this.categoryService.getCategoryById(this.id).subscribe(category => {
        this.model = category;
      })
    }
   
    this.imageSelectorSubscription = this.imageService.onSelectImage().subscribe({
      next: (response) => {
        this.model.featuredImageUrl = response.url;
        this.closeImageSelector();
      }
    })
  }

  ngOnDestroy(): void {
    this.imageSelectorSubscription?.unsubscribe();
    this.updateCategorySubscription?.unsubscribe();
    this.getCategoryByIdSubscription?.unsubscribe();
    this.imageService.resetSelectedImage();
  }
}
