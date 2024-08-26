'use client'
import { useParamsStore } from '@/hooks/useParamStore'
import { usePathname, useRouter } from 'next/navigation'
import React, { useState } from 'react'
import { FaSearch } from 'react-icons/fa'

export default function Search() {
    const setParams = useParamsStore(state => state.setParams)
    const setSearchValue = useParamsStore(state => state.setSearchValue)
    const searchValue = useParamsStore(state => state.searchValue)
    const router = useRouter();
    const pathname = usePathname();
    const [value,setValue] = useState('')

    const onChange = (event:any) => {

        setSearchValue(event.target.value)
    }

    const search = () => { 
        if(pathname !=="/") router.push("/");
        setParams({searchTerm: searchValue})
    }

  return (
    <div className='flex w-[50%] items-center border-2 rounded-full py-2 shadow-sm'> 
        <input value={searchValue} onKeyDown={(e:any) => {if(e.key === "Enter") search()}} onChange={onChange} type="text" placeholder='Search for cars by make, model or color'
        className='flex-grow pl-5 bg-transparent border-transparent focus:outline-none focus:border-transparent text-sm focus:ring-0
         text-gray-600' />
        <button onClick={search}>
            <FaSearch size={34} className='bg-red-400 text-white rounded-full p-2 cursor-pointer mx-2'/>
        </button>
    </div>
  )
}
