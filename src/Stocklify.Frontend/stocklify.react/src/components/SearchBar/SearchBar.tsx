function SearchBar() {
    return (
        <div className="search-bar flex items-center bg-gray-800 rounded px-3 py-2 w-full max-w-md">
            <svg className="size-5 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"></path></svg>
            <input 
                type="text" 
                placeholder="Search stocks..." 
                className="ml-3 bg-transparent focus:outline-none text-white w-full"
            />
        </div>
    );
}

export default SearchBar;