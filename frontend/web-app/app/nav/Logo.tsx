'use client'
import { useParamsStore } from '@/hooks/useParamStore'
import React from 'react'
import { AiOutlineCar } from 'react-icons/ai'
 

export default function Logo() {
    const reset = useParamsStore(state => state.reset)
  return (
    <div onClick={reset} className=" cursor-pointer flex items-center gap-2 font-semibold text-red-500">
    <AiOutlineCar  size={34} />
    <div>JaiSales Auction</div>
  </div>
  )
}
