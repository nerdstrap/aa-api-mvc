﻿using System;
using System.Web.Mvc;

namespace Nerdstrap.Identity.Web.Mvc.Filters
{
    /// <summary>
    /// To improve Search Engine Optimization SEO, there should only be a single URL for each resource. Case
    /// differences and/or URL's with/without trailing slashes are treated as different URL's by search engines. This
    /// filter redirects all non-canonical URL's based on the settings specified to their canonical equivalent.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class RedirectToCanonicalUrlAttribute : FilterAttribute, IAuthorizationFilter
    {
        private const char QueryCharacter = '?';
        private const char SlashCharacter = '/';

        private readonly bool appendTrailingSlash;
        private readonly bool lowercaseUrls;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectToCanonicalUrlAttribute" /> class.
        /// </summary>
        /// <param name="appendTrailingSlash">If set to <c>true</c> append trailing slashes, otherwise strip trailing
        /// slashes.</param>
        /// <param name="lowercaseUrls">If set to <c>true</c> lower-case all URL's.</param>
        public RedirectToCanonicalUrlAttribute(
            bool appendTrailingSlash,
            bool lowercaseUrls)
        {
            this.appendTrailingSlash = appendTrailingSlash;
            this.lowercaseUrls = lowercaseUrls;
        }

        /// <summary>
        /// Gets a value indicating whether to append trailing slashes.
        /// </summary>
        /// <value>
        /// <c>true</c> if appending trailing slashes; otherwise, strip trailing slashes.
        /// </value>
        public bool AppendTrailingSlash
        {
            get { return this.appendTrailingSlash; }
        }

        /// <summary>
        /// Gets a value indicating whether to lower-case all URL's.
        /// </summary>
        /// <value>
        /// <c>true</c> if lower-casing URL's; otherwise, <c>false</c>.
        /// </value>
        public bool LowercaseUrls
        {
            get { return this.lowercaseUrls; }
        }

        /// <summary>
        /// Determines whether the HTTP request contains a non-canonical URL using <see cref="TryGetCanonicalUrl"/>,
        /// if it doesn't calls the <see cref="HandleNonCanonicalRequest"/> method.
        /// </summary>
        /// <param name="filterContext">An object that encapsulates information that is required in order to use the
        /// <see cref="RedirectToCanonicalUrlAttribute"/> attribute.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="filterContext"/> parameter is <c>null</c>.</exception>
        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (string.Equals(filterContext.HttpContext.Request.HttpMethod, "GET", StringComparison.Ordinal))
            {
                string canonicalUrl;
                if (!this.TryGetCanonicalUrl(filterContext, out canonicalUrl))
                {
                    this.HandleNonCanonicalRequest(filterContext, canonicalUrl);
                }
            }
        }

        /// <summary>
        /// Determines whether the specified URl is canonical and if it is not, outputs the canonical URL.
        /// </summary>
        /// <param name="filterContext">An object that encapsulates information that is required in order to use the
        /// <see cref="RedirectToCanonicalUrlAttribute" /> attribute.</param>
        /// <param name="canonicalUrl">The canonical URL.</param>
        /// <returns><c>true</c> if the URL is canonical, otherwise <c>false</c>.</returns>
        protected virtual bool TryGetCanonicalUrl(AuthorizationContext filterContext, out string canonicalUrl)
        {
            bool isCanonical = true;

            Uri url = filterContext.HttpContext.Request.Url;
            canonicalUrl = url.ToString();
            int queryIndex = canonicalUrl.IndexOf(QueryCharacter);

            // If we are not dealing with the home page. Note, the home page is a special case and it doesn't matter
            // if there is a trailing slash or not. Both will be treated as the same by search engines.
            if (url.AbsolutePath.Length > 1)
            {
                if (queryIndex == -1)
                {
                    bool hasTrailingSlash = canonicalUrl[canonicalUrl.Length - 1] == SlashCharacter;

                    if (this.appendTrailingSlash)
                    {
                        // Append a trailing slash to the end of the URL.
                        if (!hasTrailingSlash && !this.HasNoTrailingSlashAttribute(filterContext))
                        {
                            canonicalUrl += SlashCharacter;
                            isCanonical = false;
                        }
                    }
                    else
                    {
                        // Trim a trailing slash from the end of the URL.
                        if (hasTrailingSlash)
                        {
                            canonicalUrl = canonicalUrl.TrimEnd(SlashCharacter);
                            isCanonical = false;
                        }
                    }
                }
                else
                {
                    bool hasTrailingSlash = canonicalUrl[queryIndex - 1] == SlashCharacter;

                    if (this.appendTrailingSlash)
                    {
                        // Append a trailing slash to the end of the URL but before the query string.
                        if (!hasTrailingSlash && !this.HasNoTrailingSlashAttribute(filterContext))
                        {
                            canonicalUrl = canonicalUrl.Insert(queryIndex, SlashCharacter.ToString());
                            isCanonical = false;
                        }
                    }
                    else
                    {
                        // Trim a trailing slash to the end of the URL but before the query string.
                        if (hasTrailingSlash)
                        {
                            canonicalUrl = canonicalUrl.Remove(queryIndex - 1, 1);
                            isCanonical = false;
                        }
                    }
                }
            }

            if (this.lowercaseUrls)
            {
                foreach (char character in canonicalUrl)
                {
                    if (this.HasNoLowercaseQueryStringAttribute(filterContext) && queryIndex != -1)
                    {
                        if (character == QueryCharacter)
                        {
                            break;
                        }

                        if (char.IsUpper(character) && !this.HasNoTrailingSlashAttribute(filterContext))
                        {
                            canonicalUrl = canonicalUrl.Substring(0, queryIndex).ToLower() + canonicalUrl.Substring(queryIndex, canonicalUrl.Length - queryIndex);
                            isCanonical = false;
                            break;
                        }
                    }
                    else
                    {
                        if (char.IsUpper(character) && !this.HasNoTrailingSlashAttribute(filterContext))
                        {
                            canonicalUrl = canonicalUrl.ToLower();
                            isCanonical = false;
                            break;
                        }
                    }
                }
            }

            return isCanonical;
        }

        /// <summary>
        /// Handles HTTP requests for URL's that are not canonical. Performs a 301 Permanent Redirect to the canonical URL.
        /// </summary>
        /// <param name="filterContext">An object that encapsulates information that is required in order to use the
        /// <see cref="RedirectToCanonicalUrlAttribute" /> attribute.</param>
        /// <param name="canonicalUrl">The canonical URL.</param>
        protected virtual void HandleNonCanonicalRequest(AuthorizationContext filterContext, string canonicalUrl)
        {
            filterContext.Result = new RedirectResult(canonicalUrl, true);
        }

        /// <summary>
        /// Determines whether the specified action or its controller has the <see cref="NoTrailingSlashAttribute"/>
        /// attribute specified.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// <returns><c>true</c> if a <see cref="NoTrailingSlashAttribute"/> attribute is specified, otherwise
        /// <c>false</c>.</returns>
        protected virtual bool HasNoTrailingSlashAttribute(AuthorizationContext filterContext)
        {
            return filterContext.ActionDescriptor.IsDefined(typeof(NoTrailingSlashAttribute), false) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(NoTrailingSlashAttribute), false);
        }

        /// <summary>
        /// Determines whether the specified action or its controller has the <see cref="NoLowercaseQueryStringAttribute"/>
        /// attribute specified.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// <returns><c>true</c> if a <see cref="NoLowercaseQueryStringAttribute"/> attribute is specified, otherwise
        /// <c>false</c>.</returns>
        protected virtual bool HasNoLowercaseQueryStringAttribute(AuthorizationContext filterContext)
        {
            return filterContext.ActionDescriptor.IsDefined(typeof(NoLowercaseQueryStringAttribute), false) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(NoLowercaseQueryStringAttribute), false);
        }
    }
}