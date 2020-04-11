using System;

namespace NovaPagedList
{
    /// <summary>
    /// Extension class for gathering extra information of an <see cref="IPagedList{T}"/> instance.
    /// </summary>
    public static class PagedListMetadataExtensions
    {
        /// <summary>
        /// Returns whether the current page is the first subset within the superset.
        /// </summary>
        /// <param name="metadata">The metadata of the <see cref="IPagedList{T}"/>.</param>
        /// <returns><see langword="true"/> if the current page is the first one; otherwise <see langword="false"/>.</returns>
        public static bool IsFirstPage(this IPagedListMetadata metadata)
        {
            return (metadata.PageNumber == 1);
        }

        /// <summary>
        /// Returns whether the current page is the last subset within the superset.
        /// </summary>
        /// <param name="metadata">The metadata of the <see cref="IPagedList{T}"/>.</param>
        /// <returns><see langword="true"/> if the current page is the last one; otherwise <see langword="false"/>.</returns>
        public static bool IsLastPage(this IPagedListMetadata metadata)
        {
            return (metadata.PageNumber == metadata.PageCount);
        }

        /// <summary>
        /// Returns whether the current page is not the first subset within the superset.
        /// </summary>
        /// <param name="metadata">The metadata of the <see cref="IPagedList{T}"/>.</param>
        /// <returns><see langword="true"/> if the current page is the not first one; otherwise <see langword="false"/>.</returns>
        public static bool HasPreviousPage(this IPagedListMetadata metadata)
        {
            return !metadata.IsFirstPage();
        }

        /// <summary>
        /// Returns whether the current page is not the last subset within the superset.
        /// </summary>
        /// <param name="metadata">The metadata of the <see cref="IPagedList{T}"/>.</param>
        /// <returns><see langword="true"/> if the current page is the not last one; otherwise <see langword="false"/>.</returns>
        public static bool HasNextPage(this IPagedListMetadata metadata)
        {
            return !metadata.IsLastPage();
        }
    }
}
