'use client'
import { useParamsStore } from '@/hooks/useParamStore'
import { usePathname, useRouter } from 'next/navigation'
import React from 'react'
import { AiOutlineCar } from 'react-icons/ai'
 

export default function Logo() {
  const router = useRouter();

  const pathname = usePathname();

  const doReset = () => { 
    if(pathname !== "/")
    {
      router.push("/")
      reset();
    }
  }


    const reset = useParamsStore(state => state.reset)
  return (
    <div onClick={doReset} className=" cursor-pointer flex items-center gap-2 font-semibold text-red-500">
    <AiOutlineCar  size={34} />
    <div>JaiSales Auction</div>
  </div>
  )
}
